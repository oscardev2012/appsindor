using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace SafeManagement.Data;

public sealed class SafeMailOptions
{
    public string? Host { get; set; }
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string? User { get; set; }
    public string? Password { get; set; }
    public string? From { get; set; }
    public string FromName { get; set; } = "SAFE Project Solution";
    public int MaxPerHourPerIp { get; set; } = 30;
}

public record SendEmailDto(string? To, string? Subject, string? Body, string? ReplyTo, string? ReplyName);

public sealed class SafeMailer(IOptions<SafeMailOptions> options)
{
    private readonly SafeMailOptions _o = options.Value;
    private readonly ConcurrentDictionary<string, ConcurrentQueue<DateTime>> _sent = new();

    public bool Ready => !string.IsNullOrWhiteSpace(_o.Host) && !string.IsNullOrWhiteSpace(_o.From);

    public static bool IsAddress(string? value) =>
        !string.IsNullOrWhiteSpace(value) && MailAddress.TryCreate(value.Trim(), out var a) && a.Address == value.Trim();

    public bool Allow(string sender)
    {
        var cutoff = DateTime.UtcNow.AddHours(-1);
        var log = _sent.GetOrAdd(sender, _ => new());
        while (log.TryPeek(out var at) && at < cutoff) log.TryDequeue(out _);
        if (log.Count >= _o.MaxPerHourPerIp) return false;
        log.Enqueue(DateTime.UtcNow);
        return true;
    }

    public async Task SendAsync(string to, string subject, string body, string? replyTo, string? replyName, CancellationToken ct)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(_o.From!, _o.FromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        message.To.Add(to);
        if (IsAddress(replyTo)) message.ReplyToList.Add(new MailAddress(replyTo!.Trim(), replyName?.Trim() ?? ""));

        using var smtp = new SmtpClient(_o.Host, _o.Port) { EnableSsl = _o.EnableSsl };
        if (!string.IsNullOrWhiteSpace(_o.User))
            smtp.Credentials = new NetworkCredential(_o.User, _o.Password);
        await smtp.SendMailAsync(message, ct);
    }
}
