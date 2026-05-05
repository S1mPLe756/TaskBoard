namespace SubscriptionService.Application.DTOs.Responses;

public class UserCardResponse
{
    public Guid Id { get; set; }
    public string PaymentMethodId { get; set; }
    public string Last4 { get; set; }
    public string CardType { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public int IsActive { get; set; }
}