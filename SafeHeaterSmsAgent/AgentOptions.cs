namespace SafeHeaterSmsAgent;

public class AgentOptions
{
    public string OpenAiApiKey { get; set; } = string.Empty;
    public string OpenAiModel { get; set; } = "gpt-4o-mini";
    /// <summary>Live calls. Stronger than SMS so it can listen instead of reciting a form.</summary>
    public string OpenAiVoiceModel { get; set; } = "gpt-4o";

    public string TwilioAccountSid { get; set; } = string.Empty;
    public string TwilioAuthToken { get; set; } = string.Empty;
    public string TwilioPhoneNumber { get; set; } = string.Empty;

    /// <summary>Tu jefe: recibe un SMS cuando el bot no debe seguir solo.</summary>
    public string BossPhone { get; set; } = string.Empty;

    public string BusinessName { get; set; } = "SAFE Water Heater";
    public string ServiceArea { get; set; } = "Charlotte, NC and surrounding areas (about 20 miles)";
    public string CallNumber { get; set; } = "704-839-6126";
    public string SafeEmail { get; set; } = "safeprojectsolution@gmail.com";
}
