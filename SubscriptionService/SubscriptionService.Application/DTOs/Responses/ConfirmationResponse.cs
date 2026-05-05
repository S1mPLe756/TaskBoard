namespace SubscriptionService.Application.DTOs.Responses;

public class ConfirmationResponse
{
    public string ConfirmationUrl { get; set; } = string.Empty;

    public string PaymentId { get; set; } = string.Empty;
}