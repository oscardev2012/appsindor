using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SafeHeaterSmsAgent;
using Twilio.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AgentOptions>(builder.Configuration.GetSection("Agent"));
builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    o.KnownIPNetworks.Clear();
    o.KnownProxies.Clear();
});

var sqlite = SqlitePath(builder);
builder.Services.AddDbContext<AgentDb>(o => o.UseSqlite(sqlite));
builder.Services.AddHttpClient<SalesAgent>(c => c.Timeout = TimeSpan.FromSeconds(45));
builder.Services.AddSingleton<TwilioSender>();

var app = builder.Build();
app.UseForwardedHeaders();
app.UseDefaultFiles();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AgentDb>();
    db.Database.EnsureCreated();
    foreach (var sql in new[]
    {
        "ALTER TABLE Conversations ADD COLUMN ActiveBrand TEXT NOT NULL DEFAULT ''",
        "ALTER TABLE Conversations ADD COLUMN CustomerName TEXT NOT NULL DEFAULT ''",
        "ALTER TABLE Conversations ADD COLUMN Email TEXT NOT NULL DEFAULT ''",
        "ALTER TABLE Conversations ADD COLUMN Address TEXT NOT NULL DEFAULT ''",
        "ALTER TABLE Conversations ADD COLUMN InstallWindow TEXT NOT NULL DEFAULT ''",
        "ALTER TABLE Conversations ADD COLUMN ProductSummary TEXT NOT NULL DEFAULT ''",
        "ALTER TABLE Conversations ADD COLUMN BookingNotified INTEGER NOT NULL DEFAULT 0",
        "ALTER TABLE Conversations ADD COLUMN Stage TEXT NOT NULL DEFAULT 'NEW'",
        "ALTER TABLE Conversations ADD COLUMN StateVersion INTEGER NOT NULL DEFAULT 0",
        "ALTER TABLE Conversations ADD COLUMN BookingId TEXT NOT NULL DEFAULT ''",
        "ALTER TABLE Conversations ADD COLUMN Channel TEXT NOT NULL DEFAULT 'SMS'",
        "ALTER TABLE Conversations ADD COLUMN VoiceCallSid TEXT NOT NULL DEFAULT ''",
        "ALTER TABLE Conversations ADD COLUMN VoiceStartedUtc TEXT NOT NULL DEFAULT '0001-01-01 00:00:00'",
        """
        CREATE TABLE IF NOT EXISTS FieldCorrections (
          Id INTEGER PRIMARY KEY AUTOINCREMENT,
          ConversationId INTEGER NOT NULL,
          Field TEXT NOT NULL DEFAULT '',
          OldValue TEXT NOT NULL DEFAULT '',
          NewValue TEXT NOT NULL DEFAULT '',
          AppliedStatus TEXT NOT NULL DEFAULT 'applied',
          AtUtc TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
        )
        """,
        """
        CREATE TABLE IF NOT EXISTS AlertOutbox (
          Id INTEGER PRIMARY KEY AUTOINCREMENT,
          ConversationId INTEGER NOT NULL,
          EventType TEXT NOT NULL DEFAULT '',
          Priority TEXT NOT NULL DEFAULT 'ALTA',
          Body TEXT NOT NULL DEFAULT '',
          Status TEXT NOT NULL DEFAULT 'queued',
          AtUtc TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
        )
        """
    })
    {
        try { db.Database.ExecuteSqlRaw(sql); }
        catch { /* column already exists */ }
    }
}

var inflight = new ConcurrentDictionary<string, byte>(StringComparer.Ordinal);
var voiceJobs = new ConcurrentDictionary<string, VoicePending>(StringComparer.Ordinal);
var voiceEnglish = new ConcurrentDictionary<string, bool>(StringComparer.Ordinal);

app.MapGet("/health", () => Results.Ok(new { ok = true, line = "+19808447180", mode = "sms+voice-inbound", brands = 6 }));

app.MapGet("/inbox", async (AgentDb db) =>
{
    var threads = await db.Conversations
        .Include(c => c.Messages)
        .OrderByDescending(c => c.LastMessageAtUtc)
        .Take(50)
        .ToListAsync();

    var html = new System.Text.StringBuilder();
    html.Append("<html><body style='font-family:sans-serif;max-width:720px;margin:2rem auto'>");
    html.Append("<h1>SMS inbox</h1>");
    foreach (var t in threads)
    {
        html.Append($"<h3>{t.CustomerPhone} {System.Net.WebUtility.HtmlEncode(t.ActiveBrand)} {System.Net.WebUtility.HtmlEncode(t.Stage)} {(t.NeedsHuman ? " — HUMAN" : "")} {(t.BookingNotified ? " — CITA" : "")}</h3>");
        if (!string.IsNullOrWhiteSpace(t.CustomerName) || !string.IsNullOrWhiteSpace(t.Email) || !string.IsNullOrWhiteSpace(t.Address))
            html.Append($"<p>{System.Net.WebUtility.HtmlEncode(t.CustomerName)} · {System.Net.WebUtility.HtmlEncode(t.Email)} · {System.Net.WebUtility.HtmlEncode(t.Address)} · {System.Net.WebUtility.HtmlEncode(t.InstallWindow)}</p>");
        foreach (var m in t.Messages.OrderBy(x => x.AtUtc))
        {
            if (m.Body.StartsWith("__VOICE_START__", StringComparison.Ordinal)) continue;
            html.Append($"<p><b>{m.Direction}</b> {m.AtUtc:g}<br>{System.Net.WebUtility.HtmlEncode(m.Body)}</p>");
        }
        html.Append("<hr>");
    }
    html.Append("</body></html>");
    return Results.Content(html.ToString(), "text/html");
});

app.MapPost("/sms", async (HttpRequest request, SalesAgent agent, TwilioSender sms, AgentDb db, IOptions<AgentOptions> opt, IHostEnvironment env) =>
{
    var form = await request.ReadFormAsync();
    var from = form["From"].ToString();
    var body = form["Body"].ToString();
    var sid = form["MessageSid"].ToString();
    var settings = opt.Value;
    var media = ReadMediaUrls(form);

    if (!env.IsDevelopment() && !string.IsNullOrWhiteSpace(settings.TwilioAuthToken))
    {
        var signature = request.Headers["X-Twilio-Signature"].ToString();
        var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        var validator = new RequestValidator(settings.TwilioAuthToken);
        var values = form.ToDictionary(x => x.Key, x => x.Value.ToString());
        if (!validator.Validate(url, values, signature))
        {
            return Results.Unauthorized();
        }
    }

    if (string.IsNullOrWhiteSpace(from))
    {
        return Twiml($"Thanks for texting. Call {settings.CallNumber} or tell us if this is a water heater, HVAC, roofing, realty, or insurance.");
    }

    if (string.IsNullOrWhiteSpace(body) && media.Count == 0)
    {
        return Twiml($"This line can help with a water heater, HVAC, roofing, inspection repairs, insurance, or buying/selling a home. Or call {settings.CallNumber}.");
    }

    if (!string.IsNullOrWhiteSpace(sid))
    {
        if (!inflight.TryAdd(sid, 1))
            return Twiml("");
        if (await db.ProcessedWebhooks.AnyAsync(x => x.MessageSid == sid))
        {
            inflight.TryRemove(sid, out _);
            return Twiml("");
        }
    }

    try
    {
        var turn = await agent.ReplyAsync(from, body, media, request.HttpContext.RequestAborted);

        if (!string.IsNullOrWhiteSpace(sid))
        {
            try
            {
                db.ProcessedWebhooks.Add(new ProcessedWebhook { MessageSid = sid });
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                app.Logger.LogWarning(ex, "Could not record MessageSid");
            }
        }

        if (turn.NotifyInstall && !string.IsNullOrWhiteSpace(turn.InstallSummary))
        {
            await QueueAndSendOwnerAsync(db, sms, from, turn.InstallSummary, turn.Reason ?? "ALERT", app.Logger);
        }
        else if (turn.NotifyMonitor || turn.Escalate)
        {
            try
            {
                await sms.NotifyBossAsync(from, string.IsNullOrWhiteSpace(body) ? "[photo]" : body, turn.Reason, turn.MonitorBody, turn.Brand);
            }
            catch (Exception ex)
            {
                app.Logger.LogWarning(ex, "Could not SMS the boss");
            }
        }

        var reply = string.IsNullOrWhiteSpace(turn.Reply)
            ? $"I'm checking the next option and will text you here. Or call {settings.CallNumber}."
            : turn.Reply;

        return Twiml(reply);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "SMS webhook failed");
        return Twiml($"Thanks for texting. I'm checking the next available option and will text you here. Or call {settings.CallNumber}.");
    }
    finally
    {
        if (!string.IsNullOrWhiteSpace(sid))
            inflight.TryRemove(sid, out _);
    }
});

app.MapPost("/voice", async (HttpRequest request, SalesAgent agent, IOptions<AgentOptions> opt, IHostEnvironment env) =>
{
    var form = await request.ReadFormAsync();
    if (!TwilioOk(request, opt.Value, env))
        return Results.Unauthorized();
    var from = form["From"].ToString();
    var callSid = form["CallSid"].ToString();
    try { await agent.StartVoiceCallAsync(from, callSid, CancellationToken.None); }
    catch (Exception ex) { app.Logger.LogWarning(ex, "Voice session start failed {From} {CallSid}", from, callSid); }
    if (!string.IsNullOrWhiteSpace(callSid))
        voiceEnglish[callSid] = true;
    return VoiceOpening();
});

app.MapPost("/voice/turn", async (HttpRequest request, IServiceScopeFactory scopes, TwilioSender sms, IOptions<AgentOptions> opt, IHostEnvironment env) =>
{
    var form = await request.ReadFormAsync();
    if (!TwilioOk(request, opt.Value, env))
        return Results.Unauthorized();
    var from = form["From"].ToString();
    var callSid = form["CallSid"].ToString();
    var speech = form["SpeechResult"].ToString();
    var digits = form["Digits"].ToString();
    var said = string.IsNullOrWhiteSpace(speech) ? digits : speech;
    var stickyEn = string.IsNullOrWhiteSpace(callSid) || voiceEnglish.GetValueOrDefault(callSid, true);
    var english = VoiceCallEnglish(said, stickyEn);
    if (!string.IsNullOrWhiteSpace(callSid))
        voiceEnglish[callSid] = english;
    app.Logger.LogInformation("Voice STT {From} {CallSid} {Lang}: {Said}", from, callSid, english ? "en" : "es", said);
    if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(said))
        return VoiceGather(
            "Sigo aquí. Adelante, un poco más fuerte.",
            "I'm here. Please say that again, a bit louder.",
            english);

    var jobId = Guid.NewGuid().ToString("N");
    var work = Task.Run(async () =>
    {
        using var scope = scopes.CreateScope();
        var agent = scope.ServiceProvider.GetRequiredService<SalesAgent>();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(28));
        return await agent.ReplyAsync(from, said, Array.Empty<string>(), cts.Token, "VOICE", callSid, english);
    });
    voiceJobs[jobId] = new VoicePending(work, from, said, english, 0, DateTime.UtcNow);
    return await VoiceFinishOrHold(jobId, voiceJobs, sms, app.Logger, first: true);
});

app.MapPost("/voice/wait/{jobId}", async (string jobId, HttpRequest request, TwilioSender sms, IOptions<AgentOptions> opt, IHostEnvironment env) =>
{
    _ = await request.ReadFormAsync();
    if (!TwilioOk(request, opt.Value, env))
        return Results.Unauthorized();
    return await VoiceFinishOrHold(jobId, voiceJobs, sms, app.Logger, first: false);
});

app.Run();

static string SqlitePath(WebApplicationBuilder builder)
{
    var home = Environment.GetEnvironmentVariable("HOME");
    if (!string.IsNullOrWhiteSpace(home))
    {
        var dir = Path.Combine(home, "data");
        Directory.CreateDirectory(dir);
        return $"Data Source={Path.Combine(dir, "agent.db")}";
    }

    return builder.Configuration.GetConnectionString("Agent") ?? "Data Source=agent.db";
}

static List<string> ReadMediaUrls(IFormCollection form)
{
    var list = new List<string>();
    _ = int.TryParse(form["NumMedia"].ToString(), out var n);
    for (var i = 0; i < Math.Max(n, 0) && i < 4; i++)
    {
        var u = form[$"MediaUrl{i}"].ToString();
        if (!string.IsNullOrWhiteSpace(u))
            list.Add(u);
    }
    return list;
}

static bool TwilioOk(HttpRequest request, AgentOptions settings, IHostEnvironment env)
{
    if (env.IsDevelopment() || string.IsNullOrWhiteSpace(settings.TwilioAuthToken))
        return true;
    var signature = request.Headers["X-Twilio-Signature"].ToString();
    var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
    var validator = new RequestValidator(settings.TwilioAuthToken);
    var values = request.HasFormContentType
        ? request.Form.ToDictionary(x => x.Key, x => x.Value.ToString())
        : new Dictionary<string, string>();
    return validator.Validate(url, values, signature);
}

static async Task QueueAndSendOwnerAsync(AgentDb db, TwilioSender sms, string from, string body, string eventType, ILogger logger)
{
    var row = new AlertOutbox
    {
        ConversationId = 0,
        EventType = eventType,
        Priority = body.StartsWith("[CRITICA") ? "CRITICA" : body.StartsWith("[BAJA") ? "BAJA" : "ALTA",
        Body = body,
        Status = "queued"
    };
    try
    {
        var convo = await db.Conversations.FirstOrDefaultAsync(c => c.CustomerPhone == from);
        if (convo is not null) row.ConversationId = convo.Id;
        db.AlertOutbox.Add(row);
        await db.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Could not persist owner outbox");
    }

    try
    {
        await sms.NotifyBossInstallAsync(body);
        row.Status = "sent";
        try { await db.SaveChangesAsync(); } catch { /* ignore */ }
    }
    catch (Exception ex)
    {
        row.Status = "failed";
        try { await db.SaveChangesAsync(); } catch { /* ignore */ }
        logger.LogWarning(ex, "Could not SMS the owner");
    }
}

static async Task<IResult> VoiceFinishOrHold(
    string jobId,
    ConcurrentDictionary<string, VoicePending> jobs,
    TwilioSender sms,
    ILogger logger,
    bool first)
{
    foreach (var old in jobs.Where(x => DateTime.UtcNow - x.Value.StartedUtc > TimeSpan.FromMinutes(2)).Select(x => x.Key).ToList())
        jobs.TryRemove(old, out _);

    if (!jobs.TryGetValue(jobId, out var job))
        return VoiceGather("Sigo aquí. Adelante.", "I'm here. Please go ahead.", true);

    try
    {
        var finished = await Task.WhenAny(job.Work, Task.Delay(first ? 11000 : 8000));
        if (finished == job.Work)
        {
            jobs.TryRemove(jobId, out _);
            if (!job.Work.IsCompletedSuccessfully)
            {
                logger.LogWarning(job.Work.Exception, "Voice OpenAI job failed. Heard: {Said}", job.Said);
                var fail = VoiceHeardFallback(job.Said, job.English);
                return VoiceGather(fail, fail, job.English);
            }

            var turn = await job.Work;
            if (turn.NotifyInstall && !string.IsNullOrWhiteSpace(turn.InstallSummary))
            {
                var body = turn.InstallSummary;
                _ = Task.Run(async () =>
                {
                    try { await sms.NotifyBossInstallAsync(body); }
                    catch (Exception ex) { logger.LogWarning(ex, "Voice owner SMS failed"); }
                });
            }
            else if (turn.NotifyMonitor || turn.Escalate)
            {
                var fromPhone = job.From;
                var saidText = job.Said;
                var reason = turn.Reason;
                var monitor = turn.MonitorBody;
                var brand = turn.Brand;
                _ = Task.Run(async () =>
                {
                    try { await sms.NotifyBossAsync(fromPhone, saidText, reason, monitor, brand); }
                    catch (Exception ex) { logger.LogWarning(ex, "Voice owner SMS failed"); }
                });
            }

            var spoken = string.IsNullOrWhiteSpace(turn.Reply)
                ? VoiceHeardFallback(job.Said, job.English)
                : turn.Reply;
            logger.LogInformation("Voice OpenAI ready after {Redirects} holds. Heard: {Said}", job.Redirects, job.Said);
            return VoiceGather(spoken, spoken, job.English);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Voice wait failed. Heard: {Said}", job.Said);
        jobs.TryRemove(jobId, out _);
        var fallback = VoiceHeardFallback(job.Said, job.English);
        return VoiceGather(fallback, fallback, job.English);
    }

    if (job.Redirects >= 4)
    {
        jobs.TryRemove(jobId, out _);
        var late = VoiceHeardFallback(job.Said, job.English);
        return VoiceGather(late, late, job.English);
    }

    logger.LogInformation("Voice holding for OpenAI {Job} redirect {N} heard: {Said}", jobId, job.Redirects + 1, job.Said);
    jobs[jobId] = job with { Redirects = job.Redirects + 1 };
    return VoiceHold(jobId, job.English, first);
}

static IResult VoiceHold(string jobId, bool english, bool first)
{
    var xml = new XDocument(
        new XElement("Response",
            first
                ? new XElement("Say", VoiceSayAttrs(english),
                    english ? "One moment." : "Un momento.")
                : new XElement("Pause", new XAttribute("length", "1")),
            new XElement("Redirect",
                new XAttribute("method", "POST"),
                $"/voice/wait/{jobId}")));
    return Results.Content(xml.ToString(), "application/xml");
}

static IResult VoiceOpening()
{
    var xml = new XDocument(
        new XElement("Response",
            new XElement("Gather",
                VoiceGatherAttrs(english: true, bilingualListen: true),
                new XElement("Say", VoiceSayAttrs(english: true),
                    "Hi, thanks for calling. How can I help you today?")),
            new XElement("Say", VoiceSayAttrs(english: true),
                "Sorry, I didn't catch that. Please call back, or text this number.")));
    return Results.Content(xml.ToString(), "application/xml");
}

static IResult VoiceGather(string es, string en, bool english)
{
    var raw = english
        ? (string.IsNullOrWhiteSpace(en) ? es : en)
        : (string.IsNullOrWhiteSpace(es) ? en : es);
    var say = Speakable(raw, english);
    if (say.Length > 480) say = say[..480];
    var hangup = english
        ? "I'm still here. Go ahead."
        : "Sigo aquí. Adelante.";
    var xml = new XDocument(
        new XElement("Response",
            new XElement("Gather",
                VoiceGatherAttrs(english),
                new XElement("Say", VoiceSayAttrs(english), say)),
            new XElement("Say", VoiceSayAttrs(english), hangup)));
    return Results.Content(xml.ToString(), "application/xml");
}

static string VoiceHeardFallback(string said, bool english)
{
    var t = (said ?? "").ToLowerInvariant();
    if (t.Contains("electric") || t.Contains("eléctrico") || t.Contains("electrico"))
        return HeaterPrices.Quote(!english, "electric", "", false);
    if (Regex.IsMatch(t, @"\b(50|fifty|cincuenta)\b"))
        return HeaterPrices.Quote(!english, t.Contains("gas") ? "gas" : "electric", "50", false);
    if (Regex.IsMatch(t, @"\b(40|forty|cuarenta)\b"))
        return HeaterPrices.Quote(!english, t.Contains("gas") ? "gas" : "electric", "40", false);
    if (t.Contains("gas"))
        return HeaterPrices.Quote(!english, "gas", "", false);
    return HeaterPrices.Quote(!english, "", "", false);
}

static object[] VoiceGatherAttrs(bool english, bool bilingualListen = false)
{
    var lang = english ? "en-US" : "es-US";
    var hints = bilingualListen
        ? "water heater, information, price, electric, gas, hola, español, calentador, información, precios, eléctrico, gas, agua caliente, necesito"
        : english
        ? "water heater, information, price, expensive, leak, electric, gas, forty, fifty, forty gallon, fifty gallon, HVAC, air conditioning, roofing, insurance, roof, install, appointment, yes, no, Monday, Tuesday, Wednesday"
        : "calentador, información, precio, caro, fuga, eléctrico, gas, cuarenta, cincuenta, agua caliente, techo, seguro, casa, aire, cita, sí, no, hola, necesito";
    return
    [
        new XAttribute("input", "speech"),
        new XAttribute("action", english ? "/voice/turn?lang=en" : "/voice/turn?lang=es"),
        new XAttribute("method", "POST"),
        new XAttribute("timeout", "10"),
        new XAttribute("speechTimeout", "auto"),
        new XAttribute("bargeIn", "true"),
        new XAttribute("actionOnEmptyResult", "true"),
        new XAttribute("language", lang),
        new XAttribute("hints", hints)
    ];
}

static object[] VoiceSayAttrs(bool english) =>
[
    new XAttribute("voice", english ? "Polly.Joanna-Neural" : "Polly.Lupe-Neural"),
    new XAttribute("language", english ? "en-US" : "es-US")
];

static bool VoiceCallEnglish(string said, bool stickyEnglish)
{
    var t = (said ?? "").ToLowerInvariant();
    if (t.Contains("calentador") || t.Contains("agua caliente") || t.Contains("necesito")
        || t.Contains("hola") || t.Contains("gracias") || t.Contains("español") || t.Contains("espanol")
        || t.Contains("techo") || t.Contains("cuánto") || t.Contains("cuanto")
        || t.Contains("instalación") || t.Contains("instalacion") || t.Contains("gotera")
        || t.Contains("información") || t.Contains("informacion") || t.Contains("precios")
        || t.Contains("buenos días") || t.Contains("buenos dias") || t.Contains("buenas tardes")
        || t.Contains("quiero") || t.Contains("galones") || t.Contains("eléctric") || t.Contains("electrico")
        || t.Contains("sí,") || t.Contains("buenas") || t.Contains("por favor"))
        return false;
    if (t.Contains("hello") || t.Contains("water heater") || t.Contains("i need")
        || t.Contains("how much") || t.Contains("information") || t.Contains("how can"))
        return true;
    return stickyEnglish;
}

static string Speakable(string text, bool english = true)
{
    var t = System.Text.RegularExpressions.Regex.Replace(text ?? "", @"[*_#`]+", "");
    t = System.Text.RegularExpressions.Regex.Replace(t, @"https?://\S+", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    t = HeaterPrices.ForVoice(t, english);
    t = System.Text.RegularExpressions.Regex.Replace(t, @"\s+", " ").Trim();
    return t;
}

static IResult Twiml(string message)
{
    var xml = new XDocument(
        new XElement("Response",
            string.IsNullOrWhiteSpace(message)
                ? null
                : new XElement("Message", message)));
    return Results.Content(xml.ToString(), "application/xml");
}

sealed record VoicePending(Task<AgentTurn> Work, string From, string Said, bool English, int Redirects, DateTime StartedUtc);

