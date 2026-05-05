namespace SubscriptionService.Domain.Entities;

public class UserCard
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string paymentMethodId { get; set; }
    
    public string last4 { get; set; }
    
    public string cardType { get; set; }
    
    public int expiryMonth { get; set; }
    
    public int expiryYear { get; set; }

    public bool isActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}