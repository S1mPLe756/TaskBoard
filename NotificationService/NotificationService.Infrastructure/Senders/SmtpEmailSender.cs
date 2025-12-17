using ExceptionService;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Settings;

namespace NotificationService.Infrastructure.Senders;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _settings;

    public SmtpEmailSender(IOptions<SmtpSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(string to, string subject, string message, CancellationToken ct = default)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Notification Service", _settings.From));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = message };

        using var client = new SmtpClient();

        try
        {
            var port = _settings.Port;
            var secureOption = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
            ct.ThrowIfCancellationRequested();

            await client.ConnectAsync(_settings.Host, port, secureOption);

            await client.AuthenticateAsync(_settings.Username, _settings.Password);

            await client.SendAsync(email);

            await client.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            throw new AppException($"При отправке сообщения произошла ошибка: {e.Message}");
        }
    }

    public async Task SendBulkAsync(IEnumerable<string> to, string subject, string message, CancellationToken ct = default)
    {
        var recipients = to
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .Distinct()
            .ToList();

        if (!recipients.Any())
            return;

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Notification Service", _settings.From));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = message };

        foreach (var address in recipients)
            email.To.Add(MailboxAddress.Parse(address));

        using var client = new SmtpClient();

        try
        {
            var secureOption = _settings.Port == 465
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTls;
            ct.ThrowIfCancellationRequested();

            await client.ConnectAsync(_settings.Host, _settings.Port, secureOption);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);

            await client.SendAsync(email);
            await client.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            throw new AppException($"SMTP bulk send failed: {e.Message}");
        }

    }

    public async Task CheckConnectionAsync()
    {

        using var client = new SmtpClient();

        try
        {
            var port = _settings.Port;
            var secureOption = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;

            await client.ConnectAsync(_settings.Host, port, secureOption);

            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            
            await client.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            throw new AppException($"При отправке сообщения произошла ошибка: {e.Message}");
        }
    }

}