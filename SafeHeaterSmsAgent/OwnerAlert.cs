namespace SafeHeaterSmsAgent;

internal static class OwnerAlert
{
    public static string Confirmation(Conversation convo, string brand, bool test)
    {
        var noun = Noun(brand);
        var brandName = AgentPrompt.DisplayName(brand);
        var product = string.IsNullOrWhiteSpace(convo.ProductSummary) ? noun : convo.ProductSummary;
        var when = LeadWhen.FormatEs(convo.InstallWindow);
        var tag = test ? "[TEST] " : "";
        return
            $"{tag}[{noun.ToUpperInvariant()} CONFIRMADA | {brandName}]\n" +
            $"Nombre: {Blank(convo.CustomerName)}\n" +
            $"Tel: {convo.CustomerPhone}\n" +
            $"Email: {Blank(convo.Email)}\n" +
            $"Dirección: {Blank(convo.Address)}\n" +
            $"Cuándo: {when}\n" +
            $"Asunto: {product}\n" +
            $"Acción: confirmar ventana y escribir al cliente\n" +
            $"Owner/SLA: Ricardo / 5 min";
    }

    public static string Pack(
        string priority,
        string eventType,
        Conversation convo,
        string exactTurn,
        string? meaning = null,
        string? question = null,
        string? change = null,
        string? suggested = null,
        bool test = false)
    {
        var brand = AgentPrompt.DisplayName(convo.ActiveBrand);
        var name = Blank(convo.CustomerName, "sin nombre");
        var when = string.IsNullOrWhiteSpace(convo.InstallWindow) ? "sin fecha" : LeadWhen.FormatEs(convo.InstallWindow);
        var product = string.IsNullOrWhiteSpace(convo.ProductSummary) ? brand : convo.ProductSummary;
        var tag = test ? "[TEST] " : "";
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"{tag}[{priority} | {eventType}] {brand} / {name} / {convo.CustomerPhone}");
        sb.AppendLine($"Estado: {convo.Stage} | {when}");
        sb.AppendLine($"Cliente dijo: \"{Truncate(exactTurn, 240)}\"");
        if (!string.IsNullOrWhiteSpace(question)) sb.AppendLine($"Pregunta: {question}");
        if (!string.IsNullOrWhiteSpace(change)) sb.AppendLine($"Cambio: {change}");
        if (!string.IsNullOrWhiteSpace(meaning)) sb.AppendLine($"Significado: {meaning}");
        sb.AppendLine($"Contexto: {product}; {Blank(convo.Address)}; {Blank(convo.Email)}");
        sb.AppendLine($"Acción sugerida: {suggested ?? "Ricardo revisa y responde"}");
        sb.AppendLine($"Owner/SLA: Ricardo / {(priority is "CRITICA" or "ALTA" ? "5 min" : "cuando pueda")}");
        sb.Append($"IDs: conv={convo.Id} v={convo.StateVersion}");
        if (!string.IsNullOrWhiteSpace(convo.BookingId)) sb.Append($" booking={convo.BookingId}");
        var body = sb.ToString();
        return body.Length <= 1500 ? body : body[..1500];
    }

    private static string Noun(string brand) => brand switch
    {
        "WATER_HEATER" => "instalación",
        "REALTY" => "consulta",
        "ROOFING" => "inspección",
        "HVAC" => "visita diagnóstica",
        "PROJECT" => "visita",
        "INSURANCE" => "consulta",
        _ => "cita"
    };

    private static string Blank(string? value, string fallback = "—") =>
        string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

    private static string Truncate(string value, int max) =>
        value.Length <= max ? value : value[..max];
}
