using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SafeHeaterSmsAgent;

public class TwilioSender
{
    private readonly AgentOptions _opt;
    private readonly ILogger<TwilioSender> _log;
    private bool _init;

    public TwilioSender(IOptions<AgentOptions> opt, ILogger<TwilioSender> log)
    {
        _opt = opt.Value;
        _log = log;
    }

    public async Task SendCustomerAsync(string customerPhone, string body)
    {
        if (string.IsNullOrWhiteSpace(customerPhone) || string.IsNullOrWhiteSpace(body))
            return;
        if (string.IsNullOrWhiteSpace(_opt.TwilioAccountSid) || string.IsNullOrWhiteSpace(_opt.TwilioPhoneNumber))
            return;

        EnsureClient();
        if (body.Length > 1500) body = body[..1500];

        await MessageResource.CreateAsync(
            to: new PhoneNumber(customerPhone),
            from: new PhoneNumber(_opt.TwilioPhoneNumber),
            body: body);
    }

    public async Task NotifyBossAsync(
        string customerPhone,
        string lastText,
        string? reason,
        string? monitorBody = null,
        string? brand = null)
    {
        if (string.IsNullOrWhiteSpace(_opt.BossPhone) || string.IsNullOrWhiteSpace(_opt.TwilioAccountSid))
        {
            _log.LogWarning("Cannot SMS the boss: missing BossPhone or Twilio credentials.");
            return;
        }

        EnsureClient();

        var test = PhonesMatch(customerPhone, _opt.BossPhone) ? "[TEST] " : "";
        var brandName = AgentPrompt.DisplayName(brand);
        var body =
            $"{test}[LEAD | {brandName}]\n" +
            $"Cliente: {customerPhone}\n" +
            (string.IsNullOrWhiteSpace(monitorBody) ? "" : $"{monitorBody}\n") +
            (string.IsNullOrWhiteSpace(reason) ? "" : $"Razón: {reason}\n") +
            $"Cliente escribió: {lastText}";

        if (body.Length > 1500) body = body[..1500];

        await MessageResource.CreateAsync(
            to: new PhoneNumber(_opt.BossPhone),
            from: new PhoneNumber(_opt.TwilioPhoneNumber),
            body: body);
    }

    public async Task NotifyBossInstallAsync(string body)
    {
        if (string.IsNullOrWhiteSpace(_opt.BossPhone) || string.IsNullOrWhiteSpace(_opt.TwilioAccountSid))
        {
            _log.LogWarning("Cannot SMS the boss: missing BossPhone or Twilio credentials.");
            return;
        }

        EnsureClient();
        if (string.IsNullOrWhiteSpace(body)) return;
        if (body.Length > 1500) body = body[..1500];

        await MessageResource.CreateAsync(
            to: new PhoneNumber(_opt.BossPhone),
            from: new PhoneNumber(_opt.TwilioPhoneNumber),
            body: body);
    }

    private static bool PhonesMatch(string a, string b)
    {
        static string Digits(string s) => new string(s.Where(char.IsDigit).ToArray());
        var da = Digits(a);
        var db = Digits(b);
        if (da.Length < 10 || db.Length < 10) return string.Equals(da, db, StringComparison.Ordinal);
        return da[^10..] == db[^10..];
    }

    private void EnsureClient()
    {
        if (_init) return;
        TwilioClient.Init(_opt.TwilioAccountSid, _opt.TwilioAuthToken);
        _init = true;
    }
}
