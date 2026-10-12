using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SafeHeaterSmsAgent;

public record AgentTurn(
    string Reply,
    bool Escalate,
    string? Reason,
    bool NotifyMonitor = false,
    string? MonitorBody = null,
    bool Handoff = false,
    string Brand = "",
    bool BookingConfirmed = false,
    string? CustomerName = null,
    string? Email = null,
    string? Address = null,
    string? InstallWindow = null,
    string? Product = null,
    bool NotifyInstall = false,
    string? InstallSummary = null,
    string EventClass = "",
    string PostEvent = "NONE",
    string? HumanDraft = null);

public class SalesAgent
{
    private static readonly Regex StopRx = new(
        @"^\s*(stop|stopall|unsubscribe|cancel|end|quit|end\s*all|baja|no\s+me\s+escriban|no\s+me\s+contacte|no\s+me\s+llame|d[eé]jeme\s+en\s+paz|wrong\s+number|n[uú]mero\s+equivocado)\s*[.!]?\s*$",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static readonly Regex StartRx = new(
        @"^\s*(start|unstop|activar)\s*[.!]?\s*$",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static readonly Regex HelpRx = new(
        @"^\s*(help|info|ayuda)\s*[.!]?\s*$",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static readonly Regex EmailRx = new(
        @"[A-Za-z0-9._%+\-]+@[A-Za-z0-9.\-]+\.[A-Za-z]{2,}",
        RegexOptions.CultureInvariant);

    private readonly AgentDb _db;
    private readonly HttpClient _http;
    private readonly AgentOptions _opt;
    private readonly ILogger<SalesAgent> _log;

    public SalesAgent(AgentDb db, HttpClient http, IOptions<AgentOptions> opt, ILogger<SalesAgent> log)
    {
        _db = db;
        _http = http;
        _opt = opt.Value;
        _log = log;
    }

    public async Task StartVoiceCallAsync(string customerPhone, string callSid, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(customerPhone))
            return;

        var convo = await _db.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.CustomerPhone == customerPhone, ct);

        if (convo is null)
        {
            _db.Conversations.Add(new Conversation
            {
                CustomerPhone = customerPhone,
                Channel = "VOICE",
                Stage = "NEW",
                VoiceCallSid = callSid ?? "",
                VoiceStartedUtc = DateTime.UtcNow,
                Messages = { new SmsMessage { Direction = "out", Body = $"__VOICE_START__:{callSid}" } }
            });
            await _db.SaveChangesAsync(ct);
            _log.LogInformation("Voice call started {From} {CallSid}", customerPhone, callSid);
            return;
        }

        if (!string.IsNullOrWhiteSpace(callSid)
            && string.Equals(convo.VoiceCallSid, callSid, StringComparison.Ordinal))
            return;

        ResetVoiceCallState(convo, callSid);
        await _db.SaveChangesAsync(ct);
        _log.LogInformation("Voice call reset {From} {CallSid}", customerPhone, callSid);
    }

    public async Task<AgentTurn> ReplyAsync(
        string customerPhone,
        string incoming,
        IReadOnlyList<string> mediaUrls,
        CancellationToken ct,
        string channel = "SMS",
        string callSid = "",
        bool voiceEnglish = true)
    {
        var convo = await _db.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.CustomerPhone == customerPhone, ct);

        var isNew = convo is null;
        if (convo is null)
        {
            convo = new Conversation { CustomerPhone = customerPhone, Stage = "NEW", Channel = channel };
            _db.Conversations.Add(convo);
        }

        convo.Channel = string.IsNullOrWhiteSpace(channel) ? convo.Channel : channel;
        var voice = string.Equals(channel, "VOICE", StringComparison.OrdinalIgnoreCase);
        if (voice)
        {
            if (string.IsNullOrWhiteSpace(convo.VoiceCallSid)
                || (!string.IsNullOrWhiteSpace(callSid)
                    && !string.Equals(convo.VoiceCallSid, callSid, StringComparison.Ordinal)))
                ResetVoiceCallState(convo, callSid);
            convo.VoiceSpanish = !voiceEnglish;
        }
        if (voice && convo.NeedsHuman && !IsOptOutThread(convo))
            convo.NeedsHuman = false;
        var alreadyLocked = convo.NeedsHuman && !IsOptOutThread(convo);
        var before = LeadSnap.From(convo);

        var hasPhoto = mediaUrls.Count > 0;
        if (string.IsNullOrWhiteSpace(incoming))
            incoming = hasPhoto ? "(sent a photo)" : incoming;

        var storedIn = incoming;
        if (hasPhoto) storedIn = string.IsNullOrWhiteSpace(incoming) || incoming.StartsWith('(')
            ? "[Customer sent a photo]"
            : incoming + " [photo attached]";

        convo.LastMessageAtUtc = DateTime.UtcNow;
        convo.Messages.Add(new SmsMessage { Direction = "in", Body = Truncate(storedIn, 1600) });

        if (StopRx.IsMatch(incoming) && !hasPhoto)
        {
            var already = convo.NeedsHuman;
            convo.NeedsHuman = true;
            var stopReply = LooksSpanish(incoming, convo)
                ? "Queda dado de baja. No le enviaremos más mensajes de este número. Responda START si desea reactivar."
                : "You are unsubscribed and will not receive more messages from this number. Reply START to resubscribe.";
            convo.Stage = "DNC";
            return await Finish(convo, stopReply, escalate: !already, "STOP", notify: true, ct);
        }

        if (StartRx.IsMatch(incoming) && !hasPhoto)
        {
            convo.NeedsHuman = false;
            convo.Stage = "ENGAGED";
            var startReply = LooksSpanish(incoming, convo)
                ? "Queda reactivado. ¿Es calentador de agua, aire acondicionado, techo, reparaciones de inspección, seguro o compra/venta de casa?"
                : "You're resubscribed. Is this a water heater, HVAC, roofing, inspection repairs, insurance, or buying/selling a home?";
            return await Finish(convo, startReply, escalate: false, "START", notify: false, ct);
        }

        if (HelpRx.IsMatch(incoming) && !hasPhoto && !voice)
        {
            var help = LooksSpanish(incoming, convo)
                ? $"Esta línea: calentador de agua, HVAC, techos Majestic, bienes raíces Manuela, seguros e inspecciones. Llame {_opt.CallNumber}. Tarifas de mensaje/datos pueden aplicar. Responda STOP para darse de baja."
                : $"This line: water heaters, HVAC, Majestic roofing, Manuela realty, insurance, and inspection repairs. Call {_opt.CallNumber}. Msg & data rates may apply. Reply STOP to unsubscribe.";
            return await Finish(convo, help, escalate: false, "HELP", notify: false, ct);
        }

        if (convo.NeedsHuman && IsOptOutThread(convo))
        {
            var stopReply = LooksSpanish(incoming, convo)
                ? "Queda dado de baja. Responda START si desea reactivar."
                : "You are unsubscribed. Reply START to resubscribe.";
            return await Finish(convo, stopReply, escalate: false, "DO_NOT_CONTACT", notify: false, ct);
        }

        if (alreadyLocked)
        {
            MergeLead(convo, new AgentTurn("", false, null), incoming);
            HarvestLeadFromHistory(convo);
            RecordCorrections(convo, before);
            var spanishLock = LooksSpanish(incoming, convo);
            var lockReply = spanishLock
                ? $"Un compañero ya tiene su caso y le escribe. Si es urgente, llame {_opt.CallNumber}."
                : $"A teammate already has your file and will text you. If urgent, call {_opt.CallNumber}.";
            var evt = ClassifyPost(incoming, hasPhoto, "");
            convo.StateVersion++;
            await FinishSave(convo, lockReply, ct);
            return new AgentTurn(
                lockReply, true, "HUMAN_TAKEOVER",
                NotifyInstall: true,
            InstallSummary: OwnerAlert.Pack(evt.Priority, evt.Type, convo, incoming, meaning: "IA en pausa — humano tiene el caso", suggested: "Ricardo responde", test: PhonesMatch(convo.CustomerPhone, _opt.BossPhone)),
                Brand: convo.ActiveBrand,
                PostEvent: evt.Type);
        }

        var images = voice
            ? new List<(string Mime, string B64)>()
            : await DownloadPhotosAsync(mediaUrls, ct);

        var history = (voice ? CallMessages(convo) : convo.Messages.OrderBy(m => m.AtUtc).ToList())
            .TakeLast(16)
            .Select(m => (object)new
            {
                role = m.Direction == "in" ? "user" : "assistant",
                content = (object)m.Body
            })
            .ToList();

        if (images.Count > 0 && history.Count > 0)
        {
            var parts = new List<object>
            {
                new { type = "text", text = storedIn + "\n(Look at the photo. It may be a water heater, roof, HVAC unit, or a house. Identify what it actually is and sell that sector. Use only what you can see.)" }
            };
            foreach (var img in images)
            {
                parts.Add(new
                {
                    type = "image_url",
                    image_url = new { url = $"data:{img.Mime};base64,{img.B64}" }
                });
            }

            history[^1] = new { role = "user", content = (object)parts };
        }

        var brandHint = convo.ActiveBrand;
        if (ExplicitNonWaterHeater(incoming))
            brandHint = GuessBrand(incoming) ?? "";
        else if (string.IsNullOrWhiteSpace(brandHint))
            brandHint = GuessBrand(incoming) ?? "";

        var turn = await AskModelAsync(history, brandHint, convo, ct, channel, incoming);
        if (voice)
            turn = ChooseVoiceReply(convo, incoming, turn);

        if (string.IsNullOrWhiteSpace(turn.Brand) || string.Equals(turn.Brand, "UNKNOWN", StringComparison.OrdinalIgnoreCase))
        {
            var guessed = GuessBrand(incoming);
            if (!string.IsNullOrWhiteSpace(guessed))
                turn = turn with { Brand = guessed };
        }

        if (!string.IsNullOrWhiteSpace(turn.Brand)
            && !string.Equals(turn.Brand, "UNKNOWN", StringComparison.OrdinalIgnoreCase))
            convo.ActiveBrand = turn.Brand.Trim().ToUpperInvariant();

        var mustHuman = WantsHuman(incoming) || LooksSafetyEmergency(incoming);
        if (turn.Handoff && !mustHuman)
            turn = turn with { Handoff = false };
        if (turn.Handoff && mustHuman)
            convo.NeedsHuman = true;

        if (voice && !LooksBookingIntent(incoming))
        {
            turn = turn with
            {
                BookingConfirmed = false,
                InstallWindow = null,
                CustomerName = NameOffered(incoming) ? turn.CustomerName : null,
                Address = LooksLikeAddress(incoming) ? turn.Address : null
            };
        }
        MergeLead(convo, turn, incoming);
        if (!voice)
            HarvestLeadFromHistory(convo);
        if (voice)
        {
            HarvestVoiceName(convo, incoming);
            HarvestHeaterProduct(convo, incoming);
        }
        RecordCorrections(convo, before);
        turn = ApplyBooking(convo, turn, incoming, hasPhoto);
        BumpStage(convo, voice);
        convo.StateVersion++;

        await FinishSave(convo, turn.Reply, ct);
        return turn with { NotifyMonitor = (isNew || turn.Escalate) && !turn.NotifyInstall };
    }

    private async Task<AgentTurn> Finish(
        Conversation convo,
        string reply,
        bool escalate,
        string reason,
        bool notify,
        CancellationToken ct)
    {
        await FinishSave(convo, reply, ct);
        string? pack = null;
        if (notify)
        {
            var last = convo.Messages.Where(m => m.Direction == "in").OrderByDescending(m => m.AtUtc).FirstOrDefault()?.Body ?? "";
            pack = OwnerAlert.Pack(
                reason is "STOP" or "DO_NOT_CONTACT" ? "CRITICA" : "ALTA",
                reason,
                convo,
                last,
                suggested: reason is "STOP" or "DO_NOT_CONTACT" ? "Suprimir; cero venta" : "Ricardo interviene",
                test: PhonesMatch(convo.CustomerPhone, _opt.BossPhone));
        }
        return new AgentTurn(reply, escalate, reason, notify, InstallSummary: pack, NotifyInstall: notify && pack is not null);
    }

    private async Task FinishSave(Conversation convo, string reply, CancellationToken ct)
    {
        convo.Messages.Add(new SmsMessage { Direction = "out", Body = Truncate(ClampSms(reply), 1600) });
        await _db.SaveChangesAsync(ct);
    }

    private async Task<AgentTurn> AskModelAsync(List<object> history, string? activeBrand, Conversation convo, CancellationToken ct, string channel = "SMS", string incoming = "")
    {
        if (string.IsNullOrWhiteSpace(_opt.OpenAiApiKey))
            return VoiceOrFallback(channel, incoming, convo, "OpenAI key missing");

        var voice = string.Equals(channel, "VOICE", StringComparison.OrdinalIgnoreCase);
        var model = voice
            ? (string.IsNullOrWhiteSpace(_opt.OpenAiVoiceModel) ? "gpt-4o" : _opt.OpenAiVoiceModel)
            : (string.IsNullOrWhiteSpace(_opt.OpenAiModel) ? "gpt-4o-mini" : _opt.OpenAiModel);
        var messages = new List<object> { new { role = "system", content = AgentPrompt.Build(_opt, activeBrand, LeadFile(convo, channel), channel) } };
        if (voice)
            messages.Add(new { role = "system", content = VoiceCoach(convo, incoming) });
        messages.AddRange(history);
        var payload = new
        {
            model,
            temperature = voice ? 0.7 : 0.45,
            max_tokens = voice ? 500 : 720,
            response_format = new { type = "json_object" },
            messages
        };

        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _opt.OpenAiApiKey);
        req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        if (voice)
            cts.CancelAfter(TimeSpan.FromSeconds(25));

        HttpResponseMessage res;
        try
        {
            res = await _http.SendAsync(req, cts.Token);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "OpenAI voice/SMS timeout or network");
            return VoiceOrFallback(channel, incoming, convo, "OpenAI timeout");
        }

        using (res)
        {
        var raw = await res.Content.ReadAsStringAsync(ct);
        if (!res.IsSuccessStatusCode)
        {
            _log.LogWarning("OpenAI {Status}: {Body}", (int)res.StatusCode, raw[..Math.Min(240, raw.Length)]);
            return VoiceOrFallback(channel, incoming, convo, "OpenAI error");
        }

        using var doc = JsonDocument.Parse(raw);
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(content))
            return VoiceOrFallback(channel, incoming, convo, "Empty model");


        JsonDocument parsed;
        try
        {
            parsed = JsonDocument.Parse(content);
        }
        catch (JsonException)
        {
            return VoiceOrFallback(channel, incoming, convo, "Bad JSON");
        }

        using (parsed)
        {
        var reply = parsed.RootElement.TryGetProperty("reply", out var r) ? r.GetString() : null;
        var escalate = parsed.RootElement.TryGetProperty("escalate", out var e) && e.ValueKind is JsonValueKind.True;
        var handoff = parsed.RootElement.TryGetProperty("handoff", out var h) && h.ValueKind is JsonValueKind.True;
        var reason = parsed.RootElement.TryGetProperty("reason", out var rs) ? rs.GetString() : null;
        var monitor = parsed.RootElement.TryGetProperty("monitor_es", out var m) ? m.GetString() : null;
        var temp = parsed.RootElement.TryGetProperty("temperature", out var t) ? t.GetString() : null;
        var safety = parsed.RootElement.TryGetProperty("safety", out var s) ? s.GetString() : null;
        var next = parsed.RootElement.TryGetProperty("next_action", out var n) ? n.GetString() : null;
        var missing = parsed.RootElement.TryGetProperty("missing", out var miss) ? miss.GetString() : null;
        var brand = parsed.RootElement.TryGetProperty("brand", out var b) ? b.GetString() : null;
        var service = parsed.RootElement.TryGetProperty("service_intent", out var si) ? si.GetString() : null;
        var role = parsed.RootElement.TryGetProperty("customer_role", out var cr) ? cr.GetString() : null;
        var confidence = parsed.RootElement.TryGetProperty("confidence", out var cf) ? cf.GetString() : null;
        var customerName = JsonText(parsed.RootElement, "customer_name");
        var email = JsonText(parsed.RootElement, "email");
        var address = JsonText(parsed.RootElement, "address");
        var installWindow = JsonText(parsed.RootElement, "install_window");
        var product = JsonText(parsed.RootElement, "product");
        var bookingConfirmed = parsed.RootElement.TryGetProperty("booking_confirmed", out var bc) && bc.ValueKind is JsonValueKind.True;
        var eventClass = JsonText(parsed.RootElement, "event_class");
        var postEvent = JsonText(parsed.RootElement, "post_confirmation_event");
        var humanDraft = JsonText(parsed.RootElement, "recommended_human_reply");
        brand = AgentPrompt.Normalize(brand, null);

        if (handoff && string.IsNullOrWhiteSpace(reason))
            reason = "HUMAN_HANDOFF";
        if (handoff)
            escalate = true;

        if (string.IsNullOrWhiteSpace(reply))
            return VoiceOrFallback(channel, incoming, convo, "Empty reply");

        reply = ClampSms(FixHeaterPriceSlip(reply.Trim()));

        var monitorBody = BuildMonitor(monitor, temp, safety, next, missing, reason, brand, service, role, confidence);
        return new AgentTurn(
            reply, escalate, reason, NotifyMonitor: escalate, MonitorBody: monitorBody, Handoff: handoff, Brand: brand,
            BookingConfirmed: bookingConfirmed, CustomerName: customerName, Email: email, Address: address,
            InstallWindow: installWindow, Product: product, EventClass: eventClass, PostEvent: postEvent, HumanDraft: humanDraft);
        }
        }
    }

    private AgentTurn ChooseVoiceReply(Conversation convo, string incoming, AgentTurn spoken)
    {
        var voiceHuman = WantsHuman(incoming) || LooksSafetyEmergency(incoming);
        if (voiceHuman)
        {
            convo.NeedsHuman = true;
            if (string.IsNullOrWhiteSpace(spoken.Reply) || IsHandoffDump(spoken.Reply))
                spoken = VoiceHumanLine(convo, incoming);
            return spoken with { Handoff = true, Escalate = true };
        }

        if (spoken.Handoff)
            spoken = spoken with { Handoff = false };

        if (string.IsNullOrWhiteSpace(spoken.Reply) || IsHandoffDump(spoken.Reply) || IsEmptyGreeting(spoken.Reply))
            return VoiceListenFallback(convo, incoming, "VOICE");
        return spoken;
    }

    private static bool IsEmptyGreeting(string? reply)
    {
        var t = (reply ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(t)) return true;
        return t.StartsWith("how can i help you") || t.StartsWith("how may i help")
            || t.StartsWith("hi, thanks for calling") || t.StartsWith("hi thanks for calling")
            || t.StartsWith("en qué puedo ayudarle") || t.StartsWith("en que puedo ayudarle");
    }

    private AgentTurn EnsureNewVoiceLine(Conversation convo, string incoming, AgentTurn spoken)
    {
        var last = LastBotSay(convo);
        if (string.IsNullOrWhiteSpace(last) || string.IsNullOrWhiteSpace(spoken.Reply))
            return spoken;
        if (!SameVoiceFingerprint(spoken.Reply, last))
            return spoken;
        if (string.IsNullOrWhiteSpace(convo.ProductSummary) || !convo.ProductSummary.Contains("pending", StringComparison.OrdinalIgnoreCase))
            convo.ProductSummary = string.IsNullOrWhiteSpace(convo.ProductSummary)
                ? "fuel-pending"
                : convo.ProductSummary + " fuel-pending";
        var next = VoiceAdvance(convo, incoming);
        if (!SameVoiceFingerprint(next.Reply, last) && !SameVoiceFingerprint(next.Reply, spoken.Reply))
            return next;
        var es = LooksSpanish(incoming, convo);
        return Say("VOICE", convo.ActiveBrand, HumanProblemLine(es, incoming));
    }

    private static bool SameVoiceFingerprint(string a, string b)
    {
        return VoiceFingerprint(a) is { Length: > 0 } fa
            && fa == VoiceFingerprint(b);
    }

    private static string VoiceFingerprint(string? text)
    {
        var t = (text ?? "").ToLowerInvariant();
        if (t.Contains("one thousand one hundred") && (t.Contains("electric or gas") || t.Contains("eléctrico o gas") || t.Contains("is yours electric")))
            return "quote-fuel";
        if (t.Contains("eleven ninety") && (t.Contains("electric or gas") || t.Contains("eléctrico o gas") || t.Contains("is yours electric")))
            return "quote-fuel";
        if (t.Contains("which size") || t.Contains("40 or 50") || t.Contains("40 o 50") || t.Contains("qué tamaño"))
            return "size";
        if (t.Contains("what day this week") || t.Contains("qué día esta semana"))
            return "day";
        if (t.Contains("what's your name") || t.Contains("cuál es su nombre"))
            return "name";
        if (t.Contains("what's your email") || t.Contains("cuál es su correo"))
            return "email";
        if (t.Contains("street address") || t.Contains("dirección de la casa"))
            return "addr";
        if (t.Contains("how can i help") || t.Contains("puedo ayud"))
            return "help";
        return "";
    }

    private AgentTurn VoiceHumanLine(Conversation convo, string incoming)
    {
        var es = LooksSpanish(incoming, convo);
        if (LooksSafetyEmergency(incoming))
        {
            return Say("VOICE", convo.ActiveBrand,
                es
                    ? "Si huele a gas, hay humo o monóxido, salga y llame al 911 o a la compañía de gas. No abra el equipo."
                    : "If you smell gas, see smoke, or suspect carbon monoxide, get out and call 911 or the gas company. Don't open the equipment.");
        }

        var brand = (convo.ActiveBrand ?? "").ToUpperInvariant();
        var (esLine, enLine) = brand switch
        {
            "ROOFING" => (
                "Claro. Majestic atiende en el 980-800-5991.",
                "Of course. Majestic is at 9 8 0, 8 0 0, 5 9 9 1."),
            "REALTY" => (
                "Claro. Manuela atiende en el 704-830-8486.",
                "Of course. Manuela is at 7 0 4, 8 3 0, 8 4 8 6."),
            _ => (
                $"Claro. Puede marcar al {_opt.CallNumber} para hablar con alguien de SAFE.",
                $"Of course. You can reach a person at SAFE at {SpeakPhone(_opt.CallNumber)}.")
        };
        return Say("VOICE", brand, es ? esLine : enLine);
    }

    private static string SpeakPhone(string? number)
    {
        var digits = new string((number ?? "").Where(char.IsDigit).ToArray());
        if (digits.Length >= 10)
            digits = digits[^10..];
        if (digits.Length != 10) return number ?? "";
        return $"{digits[0]} {digits[1]} {digits[2]}, {digits[3]} {digits[4]} {digits[5]}, {digits[6]} {digits[7]} {digits[8]} {digits[9]}";
    }

    private static string VoiceCoach(Conversation convo, string incoming)
    {
        var lines = CallMessages(convo)
            .TakeLast(16)
            .Select(m => m.Direction == "in" ? $"Caller: {m.Body}" : $"You: {m.Body}");
        var transcript = string.Join("\n", lines);
        if (string.IsNullOrWhiteSpace(transcript))
            transcript = "(they just heard the greeting)";
        return $"""
            Same sales brain as SMS, but you are speaking on a live call. STT is messy — infer meaning.
            THIS CALL:
            {transcript}
            They just said: "{incoming}"
            Decide what a good closer would say next. Answer that meaning, not the STT typos.
            Do not restart. Do not greet again. Do not invent facts they did not say.
            Do not recite a price list. Do not ask to schedule unless they want a day.
            If they asked for help or don't know what's wrong, help first.
            """;
    }

    private static bool RepeatsLastAsk(string? reply, string lastBot)
    {
        if (string.IsNullOrWhiteSpace(reply) || string.IsNullOrWhiteSpace(lastBot))
            return false;
        var a = reply.ToLowerInvariant();
        var b = lastBot.ToLowerInvariant();
        if (a.Contains("electric or gas") || a.Contains("eléctrico o gas") || a.Contains("electrico o gas") || a.Contains("say electric"))
            return b.Contains("electric or gas") || b.Contains("eléctrico o gas") || b.Contains("electrico o gas") || b.Contains("say electric");
        if (a.Contains("40 or 50") || a.Contains("40 o 50") || a.Contains("which size is on the tank"))
            return b.Contains("40 or 50") || b.Contains("40 o 50") || b.Contains("which size");
        if (a.Contains("what's your name") && (b.Contains("what's your name") || b.Contains("cuál es su nombre")))
            return true;
        if (a.Contains("what's your email") && (b.Contains("what's your email") || b.Contains("cuál es su correo")))
            return true;
        if (a.Contains("street address") && b.Contains("street address"))
            return true;
        if (a.Contains("how can i help") && b.Contains("how can i help"))
            return true;
        return false;
    }
    private AgentTurn VoiceOrFallback(string channel, string incoming, Conversation convo, string reason)
    {
        if (string.Equals(channel, "VOICE", StringComparison.OrdinalIgnoreCase))
            return VoiceListenFallback(convo, incoming, reason);
        return Fallback(reason, true);
    }

    private AgentTurn VoiceListenFallback(Conversation convo, string incoming, string reason)
    {
        var es = LooksSpanish(incoming, convo);
        var brand = string.IsNullOrWhiteSpace(convo.ActiveBrand) || convo.ActiveBrand == "UNKNOWN"
            ? GuessBrand(incoming) ?? ""
            : convo.ActiveBrand;
        if (!string.IsNullOrWhiteSpace(brand) && brand != "UNKNOWN")
            convo.ActiveBrand = brand;
        HarvestHeaterProduct(convo, incoming);
        var reply = brand switch
        {
            "WATER_HEATER" => es
                ? "Sí, le ayudo. Cuénteme qué está pasando con el agua caliente — ¿no sale nada, sale tibia, o hay una fuga?"
                : "Yes, I can help. What's going on with the hot water — nothing coming out, lukewarm, or a leak?",
            "HVAC" => es
                ? "Le ayudo con el aire. ¿Es frío, calor, o los dos, y qué marca el termostato?"
                : "I can help with the HVAC. Is it cooling, heat, or both, and what does the thermostat show?",
            "ROOFING" => es
                ? "Majestic puede inspeccionar el techo. No suba. ¿Es gotera, granizo, o otra cosa?"
                : "Majestic can inspect the roof. Please stay off it. Is it a leak, hail, or something else?",
            "REALTY" => es
                ? "Puedo organizar una consulta con Manuela. ¿Está buscando comprar, vender, o es su primera casa?"
                : "I can set a consult with Manuela. Are you looking to buy, sell, or is this a first home?",
            "INSURANCE" => es
                ? "Jeanna cotiza el seguro. ¿Es auto, casa, o SR-22?"
                : "Jeanna quotes the coverage. Is this auto, home, or SR-22?",
            "PROJECT" => es
                ? "Le ayudo con SAFE Project. ¿Es reparación de inspección, remodelación, o crawlspace?"
                : "I can help with SAFE Project. Is this inspection repairs, a remodel, or crawlspace?",
            _ => es
                ? "Claro, dígame qué necesita y le ayudo ahora."
                : "I can help. Tell me what's going on and I'll take it from there."
        };
        return Say(reason, brand, reply);
    }

    private static void HarvestHeaterProduct(Conversation convo, string incoming)
    {
        var t = (incoming ?? "").ToLowerInvariant();
        t = t.Replace("lecture", "electric").Replace("electro", "electric").Replace("electra", "electric");
        t = Regex.Replace(t, @"\b(a lecture|the lecture|lector|lectra|e lectric)\b", "electric");
        if (t.Contains("a metric") || t.Contains("the metric")) t += " electric";
        if (Regex.IsMatch(t, @"\b(guess|gass|gaz)\b") && BotAsked(LastBotSay(convo), "electric or gas", "eléctrico o gas"))
            t += " gas";
        var last = LastBotSay(convo);
        var unsure = Regex.IsMatch(t, @"\b(don't know|dont know|not sure|no s[eé]|whatever|either|you tell me|no idea)\b");
        if (unsure && BotAsked(last, "electric or gas", "eléctrico o gas", "40 or 50", "40 o 50"))
        {
            return;
        }
        var fuel = "";
        if (Regex.IsMatch(t, @"\b(electric|electrical|electricity|electrico|eléctrico|eléctrica|electrica)\b"))
            fuel = "electric";
        else if (Regex.IsMatch(t, @"\b(gas|propane|propano)\b"))
            fuel = "gas";
        var askedSizeNow = BotAsked(last, "40 or 50", "40 o 50", "which size", "qué tamaño", "que tamano");
        var size = "";
        if (Regex.IsMatch(t, @"\b(50|fifty|fitty|cincuenta)\s*-?\s*(gal|gallon|galones)\b")
            || (askedSizeNow && Regex.IsMatch(t, @"\b(50|fifty|fitty|cincuenta)\b"))
            || Regex.IsMatch(t, @"\bfifty\s*gallon"))
            size = "50-gal";
        else if (Regex.IsMatch(t, @"\b(40|forty|cuarenta)\s*-?\s*(gal|gallon|galones)\b")
            || (askedSizeNow && Regex.IsMatch(t, @"\b(40|forty|cuarenta)\b"))
            || Regex.IsMatch(t, @"\bforty\s*gallon"))
            size = "40-gal";
        if (string.IsNullOrWhiteSpace(fuel) && string.IsNullOrWhiteSpace(size))
            return;
        var cur = (convo.ProductSummary ?? "").ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(fuel) && (cur.Contains("electric") || cur.Contains("eléct")))
            fuel = "electric";
        if (string.IsNullOrWhiteSpace(fuel) && cur.Contains("gas") && !cur.Contains("pending"))
            fuel = "gas";
        if (string.IsNullOrWhiteSpace(size) && cur.Contains("50"))
            size = "50-gal";
        if (string.IsNullOrWhiteSpace(size) && cur.Contains("40"))
            size = "40-gal";
        var bits = new List<string>();
        if (!string.IsNullOrWhiteSpace(size)) bits.Add(size);
        if (!string.IsNullOrWhiteSpace(fuel)) bits.Add(fuel);
        if (bits.Count > 0)
            convo.ProductSummary = string.Join(" ", bits);
    }

    private static void HarvestVoiceName(Conversation convo, string incoming)
    {
        if (!string.IsNullOrWhiteSpace(convo.CustomerName)) return;
        var m = Regex.Match(incoming ?? "", @"\b(?:my name is|this is|me llamo|soy)\s+([A-Za-zÁÉÍÓÚÑáéíóúñ][A-Za-zÁÉÍÓÚÑáéíóúñ'\-]+(?:\s+[A-Za-zÁÉÍÓÚÑáéíóúñ][A-Za-zÁÉÍÓÚÑáéíóúñ'\-]+)?)", RegexOptions.IgnoreCase);
        if (m.Success && LooksLikePersonName(m.Groups[1].Value))
            convo.CustomerName = m.Groups[1].Value.Trim();
    }

    private static void ResetVoiceCallState(Conversation convo, string? callSid)
    {
        convo.VoiceCallSid = callSid ?? "";
        convo.VoiceStartedUtc = DateTime.UtcNow;
        convo.Channel = "VOICE";
        if (convo.Stage == "DNC" || IsOptOutThread(convo))
            return;

        convo.ActiveBrand = "";
        convo.ProductSummary = "";
        convo.InstallWindow = "";
        convo.CustomerName = "";
        convo.Email = "";
        convo.Address = "";
        convo.BookingNotified = false;
        convo.BookingId = "";
        convo.Stage = "NEW";
        convo.NeedsHuman = false;
        convo.Messages.Add(new SmsMessage { Direction = "out", Body = $"__VOICE_START__:{convo.VoiceCallSid}" });
    }

    private static List<SmsMessage> CallMessages(Conversation convo)
    {
        var rows = (convo.Messages ?? Array.Empty<SmsMessage>())
            .OrderBy(m => m.AtUtc)
            .ThenBy(m => m.Id)
            .Where(m => !string.IsNullOrWhiteSpace(m.Body) && !m.Body.StartsWith("__VOICE_START__:", StringComparison.Ordinal))
            .ToList();
        if (!string.Equals(convo.Channel, "VOICE", StringComparison.OrdinalIgnoreCase))
            return rows;

        var start = (convo.Messages ?? Array.Empty<SmsMessage>())
            .OrderBy(m => m.AtUtc)
            .ThenBy(m => m.Id)
            .ToList()
            .FindLastIndex(m => (m.Body ?? "").StartsWith("__VOICE_START__:", StringComparison.Ordinal));
        if (start >= 0)
            return (convo.Messages ?? Array.Empty<SmsMessage>())
                .OrderBy(m => m.AtUtc)
                .ThenBy(m => m.Id)
                .Skip(start + 1)
                .Where(m => !string.IsNullOrWhiteSpace(m.Body) && !m.Body.StartsWith("__VOICE_START__:", StringComparison.Ordinal))
                .ToList();
        if (convo.VoiceStartedUtc.Year < 2000)
            return rows;
        return rows.Where(m => m.AtUtc >= convo.VoiceStartedUtc).ToList();
    }

    private static string LastBotSay(Conversation convo) =>
        CallMessages(convo)
            .Where(m => m.Direction == "out")
            .LastOrDefault()?.Body ?? "";

    private static bool BotAsked(string bot, params string[] needles)
    {
        var t = (bot ?? "").ToLowerInvariant();
        return needles.Any(n => t.Contains(n, StringComparison.OrdinalIgnoreCase));
    }

    private AgentTurn VoiceAdvance(Conversation convo, string incoming)
    {
        var es = LooksSpanish(incoming, convo);
        var brand = string.IsNullOrWhiteSpace(convo.ActiveBrand) || convo.ActiveBrand == "UNKNOWN"
            ? GuessBrand(incoming) ?? ""
            : convo.ActiveBrand;
        if (!string.IsNullOrWhiteSpace(brand) && brand != "UNKNOWN")
            convo.ActiveBrand = brand;

        if (LooksSafetyEmergency(incoming))
            return VoiceHumanLine(convo, incoming) with { Handoff = true };

        if (brand == "WATER_HEATER")
        {
            var last = LastBotSay(convo);
            var p = (convo.ProductSummary ?? "").ToLowerInvariant();
            var fuel = p.Contains("electric") || p.Contains("eléct") ? "electric" : p.Contains("gas") ? "gas" : "";
            var size = p.Contains("50") ? "50" : p.Contains("40") ? "40" : "";
            var askedFuel = BotAsked(last, "electric or gas", "eléctrico o gas", "electrico o gas");
            var askedSize = BotAsked(last, "40 or 50", "40 o 50", "which size", "qué tamaño", "que tamano");
            var askedDay = BotAsked(last, "what day this week", "qué día esta semana", "que dia esta semana");
            var askedName = BotAsked(last, "what's your name", "cuál es su nombre", "cual es su nombre");
            var askedEmail = BotAsked(last, "what's your email", "cuál es su correo", "cual es su correo");
            var askedAddr = BotAsked(last, "street address", "dirección de la casa", "direccion de la casa");

            if (LooksHeaterLeak(incoming))
            {
                var leak = es
                    ? "Si sabe cómo, cierre la llave de agua fría. No abra el tanque. Reemplazamos calentadores con fuga. "
                    : "If you know how, shut the cold-water supply. Don't open the tank. We replace leaking heaters. ";
                if (AlreadyQuotedHeater(convo) || !LooksInfoAsk(incoming))
                    return Say("VOICE", "WATER_HEATER", leak + (es ? "¿Es eléctrico o gas?" : "Is it electric or gas?"));
                return Say("VOICE", "WATER_HEATER", leak + QuoteHeaterSpoken(es, fuel, size, objection: false));
            }

            if (LooksHeaterProblem(incoming) && !LooksInfoAsk(incoming))
                return Say("VOICE", "WATER_HEATER", VoiceProblemReply(convo, incoming));

            if (LooksPriceObjection(incoming))
                return Say("VOICE", "WATER_HEATER", QuoteHeaterSpoken(es, fuel, size, objection: true));

            if (!string.IsNullOrWhiteSpace(fuel) && string.IsNullOrWhiteSpace(size) && !askedSize)
                return Say("VOICE", "WATER_HEATER",
                    es ? "¿Es tanque de 40 o 50 galones? Diga cuarenta, o cincuenta." : "Is it a 40 or 50 gallon tank? Please say forty, or fifty.");

            if (LooksInfoAsk(incoming) && !AlreadyQuotedHeater(convo))
                return Say("VOICE", "WATER_HEATER", QuoteHeaterSpoken(es, fuel, size, objection: false));

            if (string.IsNullOrWhiteSpace(fuel) && askedFuel)
            {
                convo.ProductSummary = string.IsNullOrWhiteSpace(convo.ProductSummary)
                    ? "fuel-pending"
                    : convo.ProductSummary + " fuel-pending";
                p = (convo.ProductSummary ?? "").ToLowerInvariant();
            }

            var needFuel = string.IsNullOrWhiteSpace(fuel) && !p.Contains("fuel-pending") && !askedFuel;
            if (needFuel)
            {
                if (LooksHeaterProblem(incoming) || !LooksInfoAsk(incoming))
                    return Say("VOICE", "WATER_HEATER", VoiceProblemReply(convo, incoming));
                return Say("VOICE", "WATER_HEATER", QuoteHeaterSpoken(es, "", "", objection: false));
            }

            if (string.IsNullOrWhiteSpace(size) && !askedSize && !p.Contains("size-pending"))
                return Say("VOICE", "WATER_HEATER",
                    es ? "¿Es tanque de 40 o 50 galones? Diga cuarenta, o cincuenta." : "Is it a 40 or 50 gallon tank? Please say forty, or fifty.");
            if (string.IsNullOrWhiteSpace(size) && askedSize)
                convo.ProductSummary = (convo.ProductSummary + " size-pending").Trim();

            if (!string.IsNullOrWhiteSpace(fuel) && !string.IsNullOrWhiteSpace(size))
            {
                if (LooksBookingIntent(incoming))
                {
                    if (string.IsNullOrWhiteSpace(convo.InstallWindow) && !askedDay)
                        return Say("VOICE", "WATER_HEATER",
                            es ? "¿Qué día esta semana le sirve para instalar, mañana o tarde?" : "What day this week works for installation, morning or afternoon?");
                    if (string.IsNullOrWhiteSpace(convo.CustomerName) && !askedName)
                        return Say("VOICE", "WATER_HEATER", es ? "¿Cuál es su nombre?" : "What's your name?");
                    if (string.IsNullOrWhiteSpace(convo.Email) && !askedEmail)
                        return Say("VOICE", "WATER_HEATER", es ? "¿Cuál es su correo?" : "What's your email?");
                    if (string.IsNullOrWhiteSpace(convo.Address) && !askedAddr)
                        return Say("VOICE", "WATER_HEATER", es ? "¿Cuál es la dirección de la casa, calle y número?" : "What's the street address for the install?");
                }
                return Say("VOICE", "WATER_HEATER",
                    es
                        ? "Con eso ya sabemos qué tanque llevar. Cuando quiera le digo el precio o le tomamos el día. ¿Quiere el precio, o tiene otra pregunta?"
                        : "That tells us which tank to bring. When you're ready I can quote the price or take a day. Want the price, or do you have another question?");
            }

            return Say("VOICE", "WATER_HEATER", VoiceProblemReply(convo, incoming));
        }

        if (brand is "HVAC" or "ROOFING" or "REALTY" or "INSURANCE" or "PROJECT")
        {
            var (reply, _) = VoicePitch(brand, es);
            return Say("VOICE", brand, reply);
        }

        return Say("VOICE", "",
            es ? "¿Es un calentador de agua, o es otra cosa?" : "Is this a water heater, or something else?");
    }

    private static string QuoteHeaterSpoken(bool es, string fuel, string size, bool objection) =>
        HeaterPrices.Quote(es, fuel, size, objection);

    private static AgentTurn Say(string reason, string brand, string reply) =>
        new(reply, true, reason, Brand: brand, NotifyMonitor: true);

    private static (string Reply, string Brand) VoicePitch(string brand, bool es) => brand switch
    {
        "WATER_HEATER" => es
            ? ("Sí, le ayudo con el calentador. ¿El tanque es eléctrico o de gas?", "WATER_HEATER")
            : ("Yes, I can help with the water heater. Is the tank electric or gas?", "WATER_HEATER"),
        "HVAC" => es
            ? ("Le ayudo con el aire. ¿Es frío, calor, o los dos, y qué marca el termostato?", "HVAC")
            : ("I can help with the HVAC. Is it cooling, heat, or both, and what does the thermostat show?", "HVAC"),
        "ROOFING" => es
            ? ("Majestic puede inspeccionar el techo. No suba. ¿Qué está pasando, gotera o granizo?", "ROOFING")
            : ("Majestic can inspect the roof. Please stay off it. Is it a leak, hail, or something else?", "ROOFING"),
        "REALTY" => es
            ? ("Puedo organizar una consulta con Manuela. ¿Está buscando comprar, vender, o es su primera casa?", "REALTY")
            : ("I can set a consult with Manuela. Are you looking to buy, sell, or is this a first home?", "REALTY"),
        "INSURANCE" => es
            ? ("Jeanna cotiza el seguro. ¿Es auto, casa, o SR-22?", "INSURANCE")
            : ("Jeanna quotes the coverage. Is this auto, home, or SR-22?", "INSURANCE"),
        "PROJECT" => es
            ? ("Le ayudo con SAFE Project. ¿Es reparación de inspección, remodelación, o crawlspace?", "PROJECT")
            : ("I can help with SAFE Project. Is this inspection repairs, a remodel, or crawlspace?", "PROJECT"),
        _ => es
            ? ("Dígame un poco más de lo que necesita y le ayudo ahora.", "")
            : ("Tell me a bit more about what you need and I'll help you right now.", "")
    };

    private static bool WeakVoiceReply(string? reply)
    {
        var t = (reply ?? "").ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(t) || t.Length < 12)
            return true;
        if (SoundsLikeHumanTurn(t, ""))
            return false;
        return t.Contains("how can i help") || t.Contains("how may i help")
            || t.Contains("tell me a bit more") || t.Contains("tell me what you need")
            || t.Contains("i'm listening") || t.Contains("sigo aquí")
            || t.Contains("en qué puedo ayud")
            || t.Contains("schedule an evaluation") || t.Contains("agendar una evaluaci")
            || t.Contains("installations available this week") || t.Contains("instalaciones esta semana")
            || (t.Contains("we have install") && t.Contains("this week") && !t.Contains("one thousand") && !t.Contains("mil ciento"));
    }

    private static bool SoundsLikeHumanTurn(string? reply, string incoming)
    {
        var t = (reply ?? "").ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(t) || t.Length < 18)
            return false;
        if (t.Contains("schedule an evaluation") || t.Contains("a teammate") || t.Contains("un compañero"))
            return false;
        if ((t.Contains("50-gal") || t.Contains("50 gal") || t.Contains("50 gallon") || t.Contains("cincuenta"))
            && !Regex.IsMatch(incoming ?? "", @"\b(50|fifty|cincuenta)\b", RegexOptions.IgnoreCase))
            return false;
        var said = (incoming ?? "").ToLowerInvariant();
        if (said.Contains("agua") && (t.Contains("agua") || t.Contains("calentador") || t.Contains("caliente")))
            return true;
        if ((said.Contains("hot water") || said.Contains("heater") || said.Contains("no hot"))
            && (t.Contains("hot water") || t.Contains("heater") || t.Contains("tank")))
            return true;
        return t.Contains("sorry") || t.Contains("lamento") || t.Contains("claro que sí") || t.Contains("claro que si")
            || t.Contains("yes,") || t.Contains("i can help") || t.Contains("puedo ayud")
            || t.Contains("what's going on") || t.Contains("qué está pasando") || t.Contains("que esta pasando")
            || t.Contains("no hot water") || t.Contains("no hay agua") || t.Contains("gote")
            || t.Contains("lukewarm") || t.Contains("tibia");
    }

    private static bool LooksHeaterProblem(string incoming)
    {
        var t = (incoming ?? "").ToLowerInvariant();
        if (LooksInfoAsk(t) || LooksPriceObjection(t) || LooksHeaterLeak(t))
            return false;
        return t.Contains("problema") || t.Contains("problem") || t.Contains("no hot water") || t.Contains("no hay agua")
            || t.Contains("agua caliente") || t.Contains("hot water") || t.Contains("no sirve") || t.Contains("not working")
            || t.Contains("broken") || t.Contains("ayúdame") || t.Contains("ayudame") || t.Contains("can you help")
            || t.Contains("puedes ayudar") || t.Contains("me puedes") || t.Contains("help with")
            || t.Contains("lukewarm") || t.Contains("tibia") || t.Contains("no water")
            || t.Contains("no sale") || t.Contains("no está saliendo") || t.Contains("no esta saliendo")
            || t.Contains("not coming out") || t.Contains("calentador") || t.Contains("water heater");
    }

    private static string HumanProblemLine(bool es, string incoming) =>
        VoiceProblemReply(new Conversation { Channel = "VOICE" }, incoming);

    private static string VoiceProblemReply(Conversation convo, string incoming)
    {
        var es = LooksSpanish(incoming, convo);
        var t = (incoming ?? "").ToLowerInvariant();
        var p = (convo.ProductSummary ?? "").ToLowerInvariant();
        var fuel = p.Contains("electric") || p.Contains("eléct") ? "electric" : p.Contains("gas") ? "gas" : "";
        var size = p.Contains("50") ? "50" : p.Contains("40") ? "40" : "";
        var noHot = t.Contains("no sale") || t.Contains("no está saliendo") || t.Contains("no esta saliendo")
            || t.Contains("no hot") || t.Contains("no hay agua") || t.Contains("not coming out")
            || t.Contains("no water") || t.Contains("no le sale");

        var unsure = t.Contains("no sé") || t.Contains("no se") || t.Contains("don't know") || t.Contains("dont know")
            || t.Contains("funciona") || t.Contains("working") || t.Contains("nuevo") || t.Contains("new one")
            || t.Contains("qué está pasando") || t.Contains("que esta pasando") || t.Contains("what's going on");
        var ack = noHot || unsure
            ? (es
                ? "Sí, le ayudo. Si no hay agua caliente, casi siempre es cambio de tanque, no adivinamos una reparación. "
                : "Yes, I can help. If there's no hot water we usually replace the tank rather than guess a repair. ")
            : (es
                ? "Sí, le ayudo con el calentador. "
                : "Yes, I can help with the water heater. ");

        if (string.IsNullOrWhiteSpace(fuel))
            return ack + (es ? "¿El tanque es eléctrico o de gas?" : "Is the tank electric or gas?");
        if (string.IsNullOrWhiteSpace(size))
            return ack + (es
                ? $"Es { (fuel == "gas" ? "gas" : "eléctrico") }. ¿40 o 50 galones?"
                : $"That's {fuel}. Is it a 40 or 50 gallon tank?");
        return ack + (es
            ? "Cuando quiera le digo el precio o le tomamos el día para instalar. ¿Quiere el precio, o tiene otra pregunta?"
            : "When you're ready I can quote the price or take a day for the install. Want the price, or do you have another question?");
    }

    private static bool SpeaksHeaterPrice(string? reply)
    {
        var t = (reply ?? "").ToLowerInvariant();
        return t.Contains("one thousand") || t.Contains("eleven ninety") || t.Contains("twelve ninety")
            || t.Contains("fourteen forty") || t.Contains("fourteen ninety")
            || t.Contains("mil ciento") || t.Contains("mil doscientos") || t.Contains("mil cuatrocientos")
            || t.Contains("1,199") || t.Contains("1,299") || t.Contains("1,449") || t.Contains("1,499")
            || t.Contains("$1199") || t.Contains("$1299");
    }

    private static bool CalendarInsteadOfAnswer(string? reply, string incoming)
    {
        if (!LooksInfoAsk(incoming) && !LooksPriceObjection(incoming) && !LooksHeaterLeak(incoming))
            return false;
        var t = (reply ?? "").ToLowerInvariant();
        var saidPrice = t.Contains("1,199") || t.Contains("1199") || t.Contains("eleven ninety")
            || t.Contains("twelve ninety") || t.Contains("mil ciento") || t.Contains("1,299")
            || t.Contains("fourteen forty") || t.Contains("fourteen ninety") || t.Contains("mil cuatrocientos")
            || t.Contains("one thousand one hundred") || t.Contains("one thousand two hundred")
            || t.Contains("one thousand four hundred");
        if (saidPrice) return false;
        return t.Contains("this week") || t.Contains("esta semana") || t.Contains("available for install")
            || t.Contains("we have install");
    }

    private static bool AlreadyQuotedHeater(Conversation convo) =>
        CallMessages(convo).Any(m =>
            m.Direction == "out" &&
            (m.Body.Contains("eleven ninety", StringComparison.OrdinalIgnoreCase)
             || m.Body.Contains("twelve ninety", StringComparison.OrdinalIgnoreCase)
             || m.Body.Contains("one thousand one hundred", StringComparison.OrdinalIgnoreCase)
             || m.Body.Contains("one thousand two hundred", StringComparison.OrdinalIgnoreCase)
             || m.Body.Contains("mil ciento", StringComparison.OrdinalIgnoreCase)
             || m.Body.Contains("mil doscientos", StringComparison.OrdinalIgnoreCase)));

    private static bool LooksInfoAsk(string incoming)
    {
        var t = (incoming ?? "").ToLowerInvariant();
        return t.Contains("information") || t.Contains("informacion") || t.Contains("información")
            || t.Contains("how much") || t.Contains("cuanto") || t.Contains("cuánto")
            || t.Contains("price") || t.Contains("precio") || t.Contains("cost")
            || t.Contains("what's included") || t.Contains("que incluye") || t.Contains("qué incluye")
            || t.Contains("how does it work") || t.Contains("cómo funciona") || t.Contains("como funciona")
            || t.Contains("tell me about") || t.Contains("quiero saber") || t.Contains("need info")
            || t.Contains("what do you charge") || t.Contains("cuánto cobra") || t.Contains("cuanto cobra");
    }

    private static bool LooksBookingIntent(string incoming)
    {
        var t = (incoming ?? "").ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(t)) return false;
        if (t.Contains("agendar") || t.Contains("agéndeme") || t.Contains("agendemos")
            || t.Contains("schedule") || t.Contains("book me") || t.Contains("book it")
            || t.Contains("book the") || t.Contains("book us") || t.Contains("sign me up")
            || t.Contains("set me up") || t.Contains("set it up")
            || t.Contains("appointment") || t.Contains(" cita") || t.StartsWith("cita")
            || t.Contains("install it") || t.Contains("install this") || t.Contains("instálen")
            || t.Contains("instalen") || t.Contains("instalarlo") || t.Contains("instalarme")
            || t.Contains("come out") || t.Contains("come over") || t.Contains("when can you")
            || t.Contains("cuándo pueden") || t.Contains("cuando pueden") || t.Contains("qué día")
            || t.Contains("que dia") || t.Contains("this friday") || t.Contains("este viernes")
            || t.Contains("this monday") || t.Contains("este lunes")
            || t.Contains("in the morning") || t.Contains("in the afternoon")
            || t.Contains("por la mañana") || t.Contains("por la tarde")
            || Regex.IsMatch(t, @"\b(monday|tuesday|wednesday|thursday|friday|saturday|sunday|lunes|martes|mi[eé]rcoles|jueves|viernes|s[aá]bado|domingo)\b"))
            return true;
        return false;
    }

    private static bool LooksSchedulePush(string? reply)
    {
        var t = (reply ?? "").ToLowerInvariant();
        return t.Contains("what day") || t.Contains("qué día") || t.Contains("que dia")
            || t.Contains("qué dia") || t.Contains("morning or afternoon")
            || t.Contains("mañana o tarde") || t.Contains("manana o tarde")
            || t.Contains("we have install") || t.Contains("installs this week")
            || t.Contains("instalaciones esta semana") || t.Contains("you're all set")
            || t.Contains("ya tengo lo necesario") || t.Contains("let's get you scheduled")
            || t.Contains("get you on the calendar") || t.Contains("book you")
            || t.Contains("para agendar") || t.Contains("queda agendad")
            || t.Contains("what's your name so") || t.Contains("nombre para agendar")
            || t.Contains("available this week") || t.Contains("esta semana le sirve");
    }

    private static bool NameOffered(string incoming)
    {
        var t = incoming ?? "";
        return Regex.IsMatch(t, @"\b(my name is|this is|me llamo|soy)\s+[A-Za-zÁÉÍÓÚÑáéíóúñ]", RegexOptions.IgnoreCase);
    }

    private static bool LooksPriceObjection(string incoming)
    {
        var t = (incoming ?? "").ToLowerInvariant();
        return t.Contains("caro") || t.Contains("expensive") || t.Contains("too much")
            || t.Contains("why so much") || t.Contains("por qué tan") || t.Contains("porque tan")
            || t.Contains("cheaper") || t.Contains("más barato") || t.Contains("mas barato")
            || t.Contains("that's a lot") || t.Contains("thats a lot")
            || t.Contains("so expensive") || t.Contains("muy caro");
    }

    private static bool LooksHeaterLeak(string incoming)
    {
        var t = (incoming ?? "").ToLowerInvariant();
        if (t.Contains("roof") || t.Contains("techo") || t.Contains("shingle") || t.Contains("gotera"))
            return false;
        return t.Contains("leak") || t.Contains("leaking") || t.Contains("fuga")
            || t.Contains("goteando") || t.Contains("dripping")
            || t.Contains("water on the floor") || t.Contains("piso mojado");
    }

    private static bool IsHandoffDump(string? reply)
    {
        var t = (reply ?? "").ToLowerInvariant();
        return t.Contains("teammate") || t.Contains("compañero") || t.Contains("someone from the team")
            || t.Contains("will reach you") || t.Contains("will contact") || t.Contains("le contacta")
            || t.Contains("connect you") || t.Contains("call you back") || t.Contains("un compañero")
            || t.Contains("the team will") || t.Contains("someone will contact");
    }

    private static bool WantsHuman(string text)
    {
        var t = (text ?? "").ToLowerInvariant();
        return t.Contains("real person") || t.Contains("human") || t.Contains("representative")
            || t.Contains("quiero hablar con") || t.Contains("una persona") || t.Contains("agente humano")
            || t.Contains("speak to someone") || t.Contains("talk to a person");
    }

    private static bool LooksSafetyEmergency(string text)
    {
        var t = (text ?? "").ToLowerInvariant();
        return t.Contains("911") || t.Contains("gas leak") || t.Contains("smell gas") || t.Contains("carbon monoxide")
            || t.Contains("huelo a gas") || t.Contains("monóxido") || t.Contains("on fire");
    }

    private AgentTurn Fallback(string reason, bool escalate) => new(
        LooksSpanish("", null)
            ? $"Gracias por escribir. Estoy revisando la siguiente opción y le escribo aquí. También puede llamar al {_opt.CallNumber}."
            : $"Thanks for texting. I'm checking the next available option and will text you here. You can also call {_opt.CallNumber}.",
        escalate,
        reason,
        NotifyMonitor: escalate);

    private async Task<List<(string Mime, string B64)>> DownloadPhotosAsync(
        IReadOnlyList<string> mediaUrls,
        CancellationToken ct)
    {
        var list = new List<(string, string)>();
        if (mediaUrls.Count == 0 || string.IsNullOrWhiteSpace(_opt.TwilioAccountSid))
            return list;

        var auth = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{_opt.TwilioAccountSid}:{_opt.TwilioAuthToken}"));

        foreach (var url in mediaUrls.Take(2))
        {
            try
            {
                using var req = new HttpRequestMessage(HttpMethod.Get, url);
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                cts.CancelAfter(TimeSpan.FromSeconds(8));
                using var res = await _http.SendAsync(req, cts.Token);
                if (!res.IsSuccessStatusCode) continue;
                var bytes = await res.Content.ReadAsByteArrayAsync(cts.Token);
                if (bytes.Length is < 400 or > 4_500_000) continue;
                var mime = res.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
                if (!mime.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) continue;
                list.Add((mime, Convert.ToBase64String(bytes)));
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Could not download MMS {Url}", url);
            }
        }

        return list;
    }

    private static bool IsOptOutThread(Conversation convo)
    {
        var lastOut = convo.Messages
            .Where(m => m.Direction == "out")
            .OrderByDescending(m => m.AtUtc)
            .FirstOrDefault()?.Body ?? "";
        return lastOut.Contains("unsubscribed", StringComparison.OrdinalIgnoreCase)
            || lastOut.Contains("dado de baja", StringComparison.OrdinalIgnoreCase);
    }

    private static string? GuessBrand(string text)
    {
        var t = (text ?? "").ToLowerInvariant();
        if (t.Contains("tanque") || t.Contains("calentador") || t.Contains("water heater")
            || t.Contains("agua caliente") || t.Contains("no hot water") || t.Contains("hot water")
            || t.Contains("gallon tank") || t.Contains("galones") || t.Contains("water tank"))
            return "WATER_HEATER";
        if (t.Contains("techo") || t.Contains("gotera") || t.Contains("roof") || t.Contains("hail")
            || t.Contains("shingle") || t.Contains("granizo") || t.Contains("gutter"))
            return "ROOFING";
        if (t.Contains("realtor") || t.Contains("itin") || t.Contains("comprar") || t.Contains("vender casa")
            || t.Contains("buy a house") || t.Contains("listing") || t.Contains("lender")
            || t.Contains("taxes listos") || t.Contains("manuela") || t.Contains("first home")
            || t.Contains("primera casa") || t.Contains("comprar casa") || t.Contains("buy a home"))
            return "REALTY";
        if (t.Contains("inspecci") || t.Contains("closing") || t.Contains("due diligence") || t.Contains("inspection report"))
            return "PROJECT";
        if (t.Contains("hvac") || t.Contains("thermostat") || t.Contains("air condition") || t.Contains("no enfría")
            || t.Contains("no enfria") || t.Contains("furnace") || t.Contains("heat pump"))
            return "HVAC";
        if (t.Contains("sr-22") || t.Contains("full coverage") || t.Contains("seguro de") || t.Contains("insurance"))
            return "INSURANCE";
        if (t.Contains("crawlspace") || t.Contains("cimientos") || t.Contains("remodel") || t.Contains("adición") || t.Contains("adicion"))
            return "PROJECT";
        return null;
    }

    private static bool ExplicitNonWaterHeater(string text)
    {
        var t = (text ?? "").ToLowerInvariant();
        if (t.Contains("tanque") || t.Contains("calentador") || t.Contains("water heater") || t.Contains("hot water")
            || t.Contains("agua caliente"))
            return false;
        return GuessBrand(text ?? "") is "ROOFING" or "REALTY" or "PROJECT" or "HVAC" or "INSURANCE";
    }

    private static string BuildMonitor(
        string? monitorEs,
        string? temp,
        string? safety,
        string? next,
        string? missing,
        string? reason,
        string? brand,
        string? service,
        string? role,
        string? confidence)
    {
        var sb = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(brand) && brand != "UNKNOWN")
            sb.AppendLine($"Marca: {AgentPrompt.DisplayName(brand)}");
        if (!string.IsNullOrWhiteSpace(role)) sb.AppendLine($"Rol: {role}");
        if (!string.IsNullOrWhiteSpace(service)) sb.AppendLine($"Servicio: {service}");
        if (!string.IsNullOrWhiteSpace(confidence)) sb.AppendLine($"Confianza: {confidence}");
        if (!string.IsNullOrWhiteSpace(monitorEs)) sb.AppendLine(monitorEs.Trim());
        if (!string.IsNullOrWhiteSpace(temp)) sb.AppendLine($"Prioridad: {temp}");
        if (!string.IsNullOrWhiteSpace(safety)) sb.AppendLine($"Seguridad: {safety}");
        if (!string.IsNullOrWhiteSpace(missing)) sb.AppendLine($"Faltantes: {missing}");
        if (!string.IsNullOrWhiteSpace(next)) sb.AppendLine($"Siguiente: {next}");
        if (!string.IsNullOrWhiteSpace(reason)) sb.AppendLine($"Razón: {reason}");
        return sb.ToString().Trim();
    }

    private static bool LooksSpanish(string text, Conversation? convo)
    {
        if (convo?.VoiceSpanish == true)
            return true;
        var t = (text ?? "").ToLowerInvariant();
        if (t.Contains('á') || t.Contains('é') || t.Contains('í') || t.Contains('ó') || t.Contains('ú') || t.Contains('ñ'))
            return true;
        if (t.Contains("ayuda") || t.Contains("agua") || t.Contains("caliente") || t.Contains("fuga")
            || t.Contains("hola") || t.Contains("gracias") || t.Contains("necesito") || t.Contains("cuanto")
            || t.Contains("cuánto") || t.Contains("calentador") || t.Contains("baja")
            || t.Contains("techo") || t.Contains("casa") || t.Contains("seguro") || t.Contains("aire")
            || t.Contains("caro") || t.Contains("precio") || t.Contains("información") || t.Contains("informacion"))
            return true;

        var lastIn = (convo is not null ? CallMessages(convo) : Enumerable.Empty<SmsMessage>())
            .Where(m => m.Direction == "in")
            .Reverse()
            .Skip(1)
            .FirstOrDefault()?.Body;
        if (string.IsNullOrWhiteSpace(lastIn)) return false;
        var p = lastIn.ToLowerInvariant();
        return p.Contains("calentador") || p.Contains("agua caliente") || p.Contains("gracias") || p.Contains("hola");
    }

    private static string FixHeaterPriceSlip(string reply) => HeaterPrices.FixSlips(reply);

    private static string ClampSms(string value)
    {
        value = value.Trim();
        if (value.Length <= 420) return value;
        var cut = value.LastIndexOf('.', 419);
        if (cut < 120) cut = 417;
        return value[..(cut + 1)].Trim();
    }

    private static string JsonText(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out var el)) return "";
        return el.ValueKind == JsonValueKind.String ? el.GetString()?.Trim() ?? "" : "";
    }

    private static string LeadFile(Conversation convo, string channel = "SMS")
    {
        string Slot(string label, string value) =>
            $"{label}: {(string.IsNullOrWhiteSpace(value) ? "MISSING" : value)}";

        var today = EasternToday();
        var week = string.Join(", ", Enumerable.Range(0, 7).Select(i =>
        {
            var d = today.AddDays(i);
            return d.ToString("ddd MMM d", CultureInfo.GetCultureInfo("en-US"));
        }));

        var voiceNote = string.Equals(channel, "VOICE", StringComparison.OrdinalIgnoreCase)
            ? "LIVE CALL — empty file. Ignore SMS and older calls. Facts below are only what they said on THIS call. Do not collect name/email/address/day unless they want to book. You are listening and selling, not filling blanks."
            : "LEAD FILE FOR THIS THREAD (ask only for blanks; do not overwrite a filled field unless the customer corrected it):";

        return $"""
            TODAY in Charlotte (Eastern): {today.ToString("dddd, MMMM d, yyyy", CultureInfo.GetCultureInfo("en-US"))}.
            This week dates: {week}. If they say Friday this week, use that Friday's calendar date, not the word Friday alone.
            {voiceNote}
            {Slot("Name", convo.CustomerName)}
            {Slot("Email", convo.Email)}
            {Slot("Address", convo.Address)}
            {Slot("Install window", convo.InstallWindow)}
            {Slot("Product", convo.ProductSummary)}
            {Slot("Owner notified", convo.BookingNotified ? "YES — appointment captured. ACK=short thanks, never re-confirm. Every other turn still alerts the owner." : "no")}
            Stage: {convo.Stage}. HUMAN_TAKEOVER: {(convo.NeedsHuman ? "ON — do not sell" : "off")}.
            """;
    }

    private static void MergeLead(Conversation convo, AgentTurn turn, string incoming)
    {
        var voice = string.Equals(convo.Channel, "VOICE", StringComparison.OrdinalIgnoreCase);
        var booking = LooksBookingIntent(incoming);

        if (LooksLikePersonName(turn.CustomerName) && (!voice || booking || NameOffered(incoming)))
            convo.CustomerName = turn.CustomerName!.Trim();

        var email = FirstEmail(turn.Email) ?? FirstEmail(incoming);
        if (!string.IsNullOrWhiteSpace(email))
            convo.Email = email;

        if (LooksLikeAddress(turn.Address) && (!voice || booking || LooksLikeAddress(incoming)))
            convo.Address = turn.Address!.Trim();

        if (!voice || booking)
        {
            if (!string.IsNullOrWhiteSpace(turn.InstallWindow) && turn.InstallWindow.Length < 120)
                convo.InstallWindow = turn.InstallWindow.Trim();
            else if (string.IsNullOrWhiteSpace(convo.InstallWindow))
            {
                var window = DatedInstallWindow(null, incoming);
                if (!string.IsNullOrWhiteSpace(window))
                    convo.InstallWindow = window;
            }

            var dated = DatedInstallWindow(convo.InstallWindow, incoming);
            if (!string.IsNullOrWhiteSpace(dated))
                convo.InstallWindow = dated;
        }

        if (!string.IsNullOrWhiteSpace(turn.Product) && turn.Product.Length < 120)
            convo.ProductSummary = turn.Product.Trim();
        else if (string.IsNullOrWhiteSpace(convo.ProductSummary)
            && string.Equals(convo.ActiveBrand, "REALTY", StringComparison.OrdinalIgnoreCase))
        {
            var t = incoming.ToLowerInvariant();
            if (t.Contains("comprar") || t.Contains("primera casa") || t.Contains("buy"))
                convo.ProductSummary = "compra de casa";
            else if (t.Contains("vender") || t.Contains("sell"))
                convo.ProductSummary = "venta de casa";
        }

        var cleaned = EmailRx.Replace(incoming, " ");
        cleaned = Regex.Replace(cleaned, @"\bcorreo\b", ",", RegexOptions.IgnoreCase);

        foreach (var part in cleaned.Split(new[] { ',', '\n', ';', '|' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (string.IsNullOrWhiteSpace(convo.Email) && FirstEmail(part) is { } e)
                convo.Email = e;
            else if ((!voice || booking) && string.IsNullOrWhiteSpace(convo.Address) && LooksLikeAddress(part))
                convo.Address = part;
            else if ((!voice || booking || NameOffered(incoming)) && string.IsNullOrWhiteSpace(convo.CustomerName) && LooksLikePersonName(part))
                convo.CustomerName = part;
        }

        if ((!voice || booking) && string.IsNullOrWhiteSpace(convo.Address) && LooksLikeAddress(EmailRx.Replace(incoming, " ")))
            convo.Address = Regex.Replace(EmailRx.Replace(incoming, " "), @"\s+", " ").Trim();
        else if ((!voice || booking) && string.IsNullOrWhiteSpace(convo.CustomerName) && LooksLikePersonName(incoming))
            convo.CustomerName = incoming.Trim();
    }

    private static void HarvestLeadFromHistory(Conversation convo)
    {
        var empty = new AgentTurn("", false, null);
        var source = string.Equals(convo.Channel, "VOICE", StringComparison.OrdinalIgnoreCase)
            ? CallMessages(convo)
            : convo.Messages.OrderBy(m => m.AtUtc).ToList();
        foreach (var m in source.Where(x => x.Direction == "in").TakeLast(8))
            MergeLead(convo, empty, m.Body);
    }

    private AgentTurn ApplyBooking(Conversation convo, AgentTurn turn, string incoming, bool hasPhoto)
    {
        var brand = string.IsNullOrWhiteSpace(convo.ActiveBrand) ? turn.Brand : convo.ActiveBrand;
        if (!IsBookable(brand) && !convo.BookingNotified)
            return turn;

        var spanish = LooksSpanish(incoming, convo);
        var voice = string.Equals(convo.Channel, "VOICE", StringComparison.OrdinalIgnoreCase);
        if (voice && !LooksBookingIntent(incoming) && string.IsNullOrWhiteSpace(convo.InstallWindow) && !convo.BookingNotified)
            return turn;

        if (convo.BookingNotified)
        {
            var evt = ClassifyPost(incoming, hasPhoto, turn.PostEvent);
            var reply = evt.Type is "ACK"
                ? ClampSms(ThanksAfterBooking(brand, spanish))
                : ClampSms(ClaimsInstallBooked(turn.Reply) ? ThanksAfterBooking(brand, spanish) : turn.Reply);
            var change = LastCorrectionLine(convo);
            var alert = OwnerAlert.Pack(
                evt.Priority,
                "POST_CONFIRMATION_" + evt.Type,
                convo,
                incoming,
                meaning: string.IsNullOrWhiteSpace(turn.MonitorBody) ? evt.Meaning : turn.MonitorBody,
                question: evt.Type == "QUESTION" ? incoming : null,
                change: change,
                suggested: string.IsNullOrWhiteSpace(turn.HumanDraft) ? evt.Suggested : turn.HumanDraft,
                test: PhonesMatch(convo.CustomerPhone, _opt.BossPhone));
            return turn with
            {
                Reply = reply,
                Escalate = evt.Priority is "ALTA" or "CRITICA",
                Reason = evt.Type,
                NotifyInstall = true,
                InstallSummary = alert,
                PostEvent = evt.Type
            };
        }

        if (!HasCompleteInstallLead(convo) && (turn.BookingConfirmed || ClaimsInstallBooked(turn.Reply)))
        {
            return turn with
            {
                Reply = ClampSms(AskMissingLead(convo, brand, spanish)),
                BookingConfirmed = false,
                NotifyInstall = false
            };
        }

        if (!HasCompleteInstallLead(convo))
            return turn;

        convo.InstallWindow = DatedInstallWindow(convo.InstallWindow, incoming) ?? convo.InstallWindow;
        convo.BookingNotified = true;
        convo.Stage = "APPOINTMENT_CONFIRMED";
        if (string.IsNullOrWhiteSpace(convo.BookingId))
            convo.BookingId = $"{brand}-{convo.Id}-{DateTime.UtcNow:yyyyMMddHHmm}";
        var confirm = ClampSms(ConfirmBookingToCustomer(convo, brand, spanish));

        return turn with
        {
            Reply = confirm,
            Escalate = true,
            Reason = "BOOKING_CONFIRMED",
            BookingConfirmed = true,
            NotifyInstall = true,
            InstallSummary = OwnerAlert.Confirmation(convo, brand, PhonesMatch(convo.CustomerPhone, _opt.BossPhone))
        };
    }

    private static bool IsBookable(string? brand) =>
        brand is "WATER_HEATER" or "REALTY" or "ROOFING" or "HVAC" or "PROJECT" or "INSURANCE";

    private static bool IsThanks(string? incoming)
    {
        var t = (incoming ?? "").Trim().ToLowerInvariant();
        if (t.Length > 80) return false;
        return t is "gracias" or "muchas gracias" or "mil gracias" or "thanks" or "thank you" or "thanks!" or "ok" or "ok." or "okay"
            or "perfecto" or "perfecto." or "de acuerdo" or "listo" or "👍" or "ok gracias"
            || Regex.IsMatch(t, @"^(muchas\s+)?gracias[.! ]*$")
            || Regex.IsMatch(t, @"^thank(s| you)[.! ]*$")
            || Regex.IsMatch(t, @"^(ok|okay|perfecto)([.! ]|gracias)*$");
    }

    private static string ThanksAfterBooking(string brand, bool spanish) => (brand, spanish) switch
    {
        ("REALTY", true) => "Con gusto. El equipo de Manuela ya tiene su consulta.",
        ("REALTY", false) => "You're welcome. Manuela's team has your consultation.",
        ("ROOFING", true) => "Con gusto. Majestic ya tiene la inspección.",
        ("ROOFING", false) => "You're welcome. Majestic has the inspection request.",
        ("WATER_HEATER", true) => "Con gusto. El equipo ya tiene la instalación.",
        ("WATER_HEATER", false) => "You're welcome. The crew request is with the team.",
        (_, true) => "Con gusto.",
        _ => "You're welcome."
    };

    private static (string Es, string En) BookingNoun(string brand) => brand switch
    {
        "WATER_HEATER" => ("instalación", "installation"),
        "REALTY" => ("consulta", "consultation"),
        "ROOFING" => ("inspección", "inspection"),
        "HVAC" => ("visita diagnóstica", "diagnostic visit"),
        "PROJECT" => ("visita", "visit"),
        "INSURANCE" => ("consulta de seguro", "consultation"),
        _ => ("cita", "appointment")
    };

    private static bool HasCompleteInstallLead(Conversation convo) =>
        !string.IsNullOrWhiteSpace(convo.CustomerName)
        && !string.IsNullOrWhiteSpace(convo.Email)
        && !string.IsNullOrWhiteSpace(convo.Address)
        && !string.IsNullOrWhiteSpace(convo.InstallWindow);

    private static string AskMissingLead(Conversation convo, string brand, bool spanish)
    {
        var noun = BookingNoun(brand);
        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(convo.CustomerName))
            missing.Add(spanish ? "nombre completo" : "full name");
        if (string.IsNullOrWhiteSpace(convo.Email))
            missing.Add(spanish ? "correo electrónico" : "email");
        if (string.IsNullOrWhiteSpace(convo.Address))
            missing.Add(spanish ? "dirección" : "street address");
        if (string.IsNullOrWhiteSpace(convo.InstallWindow))
            missing.Add(spanish ? "día y mañana/tarde" : "day plus morning or afternoon");

        var list = string.Join(", ", missing);
        if (spanish)
            return $"Para dejar la {noun.Es} en el calendario, envíeme {list} en un solo mensaje.";
        return $"To lock the {noun.En} this week, send {list} in one text.";
    }

    private static string ConfirmBookingToCustomer(Conversation convo, string brand, bool spanish)
    {
        var when = LeadWhen.Format(convo.InstallWindow, spanish);
        var noun = BookingNoun(brand);
        var product = string.IsNullOrWhiteSpace(convo.ProductSummary) ? "" : $" ({convo.ProductSummary})";
        if (spanish)
            return $"Listo, {convo.CustomerName}. Dejamos su {noun.Es}{product} para el {when} en {convo.Address}. Un compañero confirma la ventana y le escribe a {convo.Email}.";
        return $"You're set, {convo.CustomerName}. We have your {noun.En}{product} for {when} at {convo.Address}. A teammate will confirm the window and email {convo.Email}.";
    }

    private static bool ClaimsInstallBooked(string reply)
    {
        var t = (reply ?? "").ToLowerInvariant();
        return t.Contains("i've scheduled") || t.Contains("i have scheduled") || t.Contains("we've scheduled")
            || t.Contains("scheduled your") || t.Contains("booked the") || t.Contains("booked your")
            || t.Contains("is scheduled") || t.Contains("is confirmed") || t.Contains("está confirmada")
            || t.Contains("esta confirmada") || t.Contains("está programada") || t.Contains("esta programada")
            || t.Contains("he programado") || t.Contains("queda agendada") || t.Contains("queda programada")
            || t.Contains("queda confirmada") || t.Contains("instalación confirmada") || t.Contains("instalacion confirmada")
            || t.Contains("consulta está") || t.Contains("consulta esta") || t.Contains("consulta confirmada")
            || t.Contains("confirmation will be sent") || t.Contains("confirmación se enviará")
            || t.Contains("hablaremos pronto");
    }

    private static string? FirstEmail(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var m = EmailRx.Match(text);
        return m.Success ? m.Value.Trim() : null;
    }

    private static bool LooksLikePersonName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        var s = value.Trim();
        if (s.Length is < 2 or > 80) return false;
        if (s.Contains('@') || EmailRx.IsMatch(s)) return false;
        if (Regex.IsMatch(s, @"\d{2,}")) return false;
        var t = s.ToLowerInvariant();
        if (t.Contains("gracias") || t.Contains("thank") || t.Contains("de nada") || t.Contains("please"))
            return false;
        if (t is "yes" or "ok" or "okay" or "sure" or "no" or "si" or "sí" or "hola" or "hello" or "hi")
            return false;
        if (Regex.IsMatch(t, @"\b(friday|monday|tuesday|wednesday|thursday|saturday|sunday|lunes|martes|miercoles|miércoles|jueves|viernes|s[aá]bado|domingo|hoy|mañana)\b"))
            return false;
        var parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length is >= 1 and <= 5 && parts.All(p => p.All(ch => char.IsLetter(ch) || ch is '-' or '\''));
    }

    private static bool LooksLikeAddress(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        var s = value.Trim();
        if (s.Length < 8 || s.Contains('@')) return false;
        if (Regex.IsMatch(s, @"^\d+\s+\S+")) return true;
        return Regex.IsMatch(s, @"\b\d{5}\b") && Regex.IsMatch(
            s,
            @"\b(st|rd|ave|ln|dr|blvd|ct|way|lane|street|road|drive|charlotte|nc)\b",
            RegexOptions.IgnoreCase);
    }

    private static string? DatedInstallWindow(string? stored, string? extra = null)
    {
        var parsed = ParseInstallWhen($"{stored} {extra}");
        if (parsed is null)
            return string.IsNullOrWhiteSpace(stored) ? null : stored.Trim();

        return parsed.Value.Part switch
        {
            "morning" => $"{parsed.Value.Date:yyyy-MM-dd} morning{ClockTag(parsed.Value.Clock)}",
            "afternoon" => $"{parsed.Value.Date:yyyy-MM-dd} afternoon{ClockTag(parsed.Value.Clock)}",
            _ => $"{parsed.Value.Date:yyyy-MM-dd}{ClockTag(parsed.Value.Clock)}"
        };
    }

    private static string ClockTag(string clock) =>
        string.IsNullOrWhiteSpace(clock) ? "" : " " + clock;

    private static string FormatInstallWhen(string? stored, bool spanish)
    {
        var parsed = ParseInstallWhen(stored);
        if (parsed is null)
            return string.IsNullOrWhiteSpace(stored)
                ? (spanish ? "fecha por confirmar" : "date to confirm")
                : stored.Trim();

        var date = parsed.Value.Date;
        var part = parsed.Value.Part;
        var clock = parsed.Value.Clock;
        if (spanish)
        {
            var es = CultureInfo.GetCultureInfo("es-ES");
            var dayName = date.ToString("dddd", es);
            dayName = char.ToUpper(dayName[0], es) + dayName[1..];
            var month = date.ToString("MMMM", es);
            var core = $"{dayName} {date.Day} de {month} de {date.Year}";
            var band = part switch
            {
                "morning" => ", en la mañana",
                "afternoon" => ", en la tarde",
                _ => ""
            };
            var clockEs = string.IsNullOrWhiteSpace(clock) ? "" : $" {clock.Replace("after ", "después de las ").Replace("pm", "p.m.")}";
            return core + band + clockEs;
        }

        var en = CultureInfo.GetCultureInfo("en-US");
        var coreEn = date.ToString("dddd, MMMM d, yyyy", en);
        var bandEn = part switch
        {
            "morning" => " in the morning",
            "afternoon" => " in the afternoon",
            _ => ""
        };
        var clockEn = string.IsNullOrWhiteSpace(clock) ? "" : $" {clock}";
        return coreEn + bandEn + clockEn;
    }

    private readonly record struct ParsedInstall(DateTime Date, string Part, string Clock);

    private static ParsedInstall? ParseInstallWhen(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var t = text.ToLowerInvariant();
        var today = EasternToday();
        DateTime? date = null;

        var iso = Regex.Match(text, @"\b(20\d{2})-(\d{2})-(\d{2})\b");
        if (iso.Success && DateTime.TryParseExact(iso.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var isoDate))
            date = isoDate.Date;

        if (date is null)
        {
            var en = Regex.Match(t, @"\b(january|february|march|april|may|june|july|august|september|october|november|december)\s+(\d{1,2})(?:st|nd|rd|th)?,?\s*(20\d{2})?\b");
            if (en.Success && DateTime.TryParse(en.Value, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out var enDate))
                date = (en.Groups[3].Success ? enDate : new DateTime(today.Year, enDate.Month, enDate.Day)).Date;
        }

        if (date is null)
        {
            var es = Regex.Match(t, @"\b(\d{1,2})\s+de\s+(enero|febrero|marzo|abril|mayo|junio|julio|agosto|septiembre|octubre|noviembre|diciembre)(?:\s+de\s+(20\d{2}))?\b");
            if (es.Success)
            {
                var months = new[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" };
                var month = Array.IndexOf(months, es.Groups[2].Value) + 1;
                var day = int.Parse(es.Groups[1].Value);
                var year = es.Groups[3].Success ? int.Parse(es.Groups[3].Value) : today.Year;
                date = new DateTime(year, month, day);
            }
        }

        var dow =
            t.Contains("friday") || t.Contains("viernes") ? DayOfWeek.Friday :
            t.Contains("monday") || t.Contains("lunes") ? DayOfWeek.Monday :
            t.Contains("tuesday") || t.Contains("martes") ? DayOfWeek.Tuesday :
            t.Contains("wednesday") || t.Contains("miércoles") || t.Contains("miercoles") ? DayOfWeek.Wednesday :
            t.Contains("thursday") || t.Contains("jueves") ? DayOfWeek.Thursday :
            t.Contains("saturday") || t.Contains("sábado") || t.Contains("sabado") ? DayOfWeek.Saturday :
            t.Contains("sunday") || t.Contains("domingo") ? DayOfWeek.Sunday :
            (DayOfWeek?)null;

        if (date is null)
        {
            var nextWeek = t.Contains("next week") || t.Contains("próxima semana") || t.Contains("proxima semana")
                || t.Contains("la otra semana") || t.Contains("semana que viene");
            if (Regex.IsMatch(t, @"\btoday\b") || Regex.IsMatch(t, @"\bhoy\b"))
                date = today;
            else if (Regex.IsMatch(t, @"\btomorrow\b") || (t.Contains("mañana") && dow is null && !t.Contains("en la mañana")))
                date = today.AddDays(1);
            else if (dow is { } d)
                date = NextOnOrAfter(nextWeek ? today.AddDays(7) : today, d);
        }

        if (date is null) return null;

        var am = t.Contains("morning") || t.Contains("en la mañana") || t.Contains("in the morning");
        var pm = t.Contains("afternoon") || t.Contains("tarde") || t.Contains("evening") || t.Contains("in the afternoon");
        var part = am && !pm ? "morning" : pm && !am ? "afternoon" : "";
        var clock = ParseClock(t);
        if (string.IsNullOrWhiteSpace(part) && !string.IsNullOrWhiteSpace(clock))
            part = "afternoon";
        return new ParsedInstall(date.Value.Date, part, clock);
    }

    private static string ParseClock(string t)
    {
        var m = Regex.Match(t, @"(?:despu[eé]s de las|after|a las)\s+(\d{1,2})");
        if (!m.Success)
            m = Regex.Match(t, @"\b(\d{1,2})\s*(?:pm|p\.m\.)\b");
        if (!m.Success) return "";
        var hour = int.Parse(m.Groups[1].Value);
        if (hour is < 1 or > 12) return "";
        return $"after {hour}pm";
    }

    private static DateTime EasternToday()
    {
        TimeZoneInfo tz;
        try { tz = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); }
        catch (TimeZoneNotFoundException)
        {
            tz = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        }
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz).Date;
    }

    private static DateTime NextOnOrAfter(DateTime from, DayOfWeek day)
    {
        var add = ((int)day - (int)from.DayOfWeek + 7) % 7;
        return from.AddDays(add);
    }

    private static bool PhonesMatch(string a, string b)
    {
        static string Digits(string s) => new string(s.Where(char.IsDigit).ToArray());
        var da = Digits(a);
        var db = Digits(b);
        if (da.Length < 10 || db.Length < 10) return string.Equals(da, db, StringComparison.Ordinal);
        return da[^10..] == db[^10..];
    }

    private void RecordCorrections(Conversation convo, LeadSnap before)
    {
        if (convo.Id == 0) return;
        void One(string field, string oldVal, string newVal)
        {
            if (string.IsNullOrWhiteSpace(newVal) || string.Equals(oldVal, newVal, StringComparison.OrdinalIgnoreCase))
                return;
            _db.FieldCorrections.Add(new FieldCorrection
            {
                ConversationId = convo.Id,
                Field = field,
                OldValue = oldVal ?? "",
                NewValue = newVal,
                AppliedStatus = "applied"
            });
        }

        One("CustomerName", before.Name, convo.CustomerName);
        One("Email", before.Email, convo.Email);
        One("Address", before.Address, convo.Address);
        One("InstallWindow", before.Window, convo.InstallWindow);
    }

    private string? LastCorrectionLine(Conversation convo)
    {
        var last = _db.FieldCorrections.Local
            .OrderByDescending(c => c.AtUtc)
            .FirstOrDefault();
        if (last is null) return null;
        if (string.IsNullOrWhiteSpace(last.OldValue))
            return $"{last.Field}={last.NewValue}";
        return $"{last.Field} {last.OldValue}→{last.NewValue} (aplicado)";
    }

    private static void BumpStage(Conversation convo, bool voice = false)
    {
        if (convo.Stage is "DNC") return;
        if (convo.NeedsHuman) { convo.Stage = "HUMAN_TAKEOVER"; return; }
        if (convo.BookingNotified) { convo.Stage = "APPOINTMENT_CONFIRMED"; return; }
        if (HasCompleteInstallLead(convo)) { convo.Stage = "BOOKING_PENDING"; return; }
        if (!string.IsNullOrWhiteSpace(convo.ActiveBrand) || !string.IsNullOrWhiteSpace(convo.CustomerName))
            convo.Stage = "QUALIFYING";
        else if (convo.Stage == "NEW" && (voice ? CallMessages(convo).Count : convo.Messages.Count) > 2)
            convo.Stage = "ENGAGED";
    }

    private static (string Type, string Priority, string Meaning, string Suggested) ClassifyPost(
        string incoming, bool hasPhoto, string modelPost)
    {
        var t = (incoming ?? "").ToLowerInvariant();
        if (hasPhoto)
            return ("MEDIA", "ALTA", "Foto/documento después de la cita", "Revisar archivo y responder");
        if (t.Contains("cancelar") && (t.Contains("cita") || t.Contains("consulta") || t.Contains("instal")))
            return ("CANCEL_DNC", "CRITICA", "Cancelación", "Cancelar y avisar al owner");
        if (t.Contains("humo") || t.Contains("fuego") || t.Contains("olor a gas") || t.Contains("colaps")
            || t.Contains("enojad") || t.Contains("frustrad") || t.Contains("agua cerca"))
            return ("RISK", "CRITICA", "Riesgo o frustración", "Ricardo contacta ahora");
        if (t.Contains("no es") || t.Contains("correcta es") || t.Contains("cambio") || t.Contains("mejor el")
            || t.Contains("actually") || t.Contains("address is") || t.Contains("correo es"))
            return ("CHANGE", "ALTA", "Cambio de dato post-cita", "Aplicar delta y confirmar con el cliente");
        if (t.Contains('?') || t.Contains("qué") || t.Contains("cómo") || t.Contains("cuándo") || t.Contains("cuando")
            || t.Contains("how ") || t.Contains("what ") || t.Contains("when "))
            return ("QUESTION", "ALTA", "Pregunta después de confirmar", "Responder con contexto de la cita");
        if (IsThanks(incoming) || modelPost.Equals("ACK", StringComparison.OrdinalIgnoreCase))
            return ("ACK", "BAJA", "Agradecimiento; no requiere intervención", "Ninguna — solo registro");
        if (!string.IsNullOrWhiteSpace(modelPost) && !modelPost.Equals("NONE", StringComparison.OrdinalIgnoreCase))
        {
            var p = modelPost.ToUpperInvariant();
            var pri = p is "RISK" or "CANCEL_DNC" ? "CRITICA" : p == "ACK" ? "BAJA" : "ALTA";
            return (p, pri, "Turno post-cita", "Ricardo revisa");
        }
        return ("QUESTION", "ALTA", "Turno posterior a la cita", "Revisar y responder");
    }

    private readonly record struct LeadSnap(string Name, string Email, string Address, string Window)
    {
        public static LeadSnap From(Conversation c) => new(c.CustomerName, c.Email, c.Address, c.InstallWindow);
    }

    private static string Truncate(string value, int max) =>
        value.Length <= max ? value : value[..max];
}

