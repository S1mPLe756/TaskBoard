namespace NotificationService.Domain.Interfaces;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string message, CancellationToken ct = default);
    Task SendBulkAsync(IEnumerable<string> to, string subject, string message, CancellationToken ct = default);
    Task CheckConnectionAsync();
}