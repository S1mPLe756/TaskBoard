using System.Net.Mail;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Senders;

namespace NotificationService.API.HealthChecks;

public class SmtpHealthCheck : IHealthCheck
{
    private IEmailSender _smtpEmailSender;

    public SmtpHealthCheck(IEmailSender sender)
    {
        _smtpEmailSender = sender;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        
        try
        {
            _smtpEmailSender.CheckConnectionAsync();

            return Task.FromResult(HealthCheckResult.Healthy("SMTP доступен"));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("SMTP недоступен", ex));
        }
    }
}