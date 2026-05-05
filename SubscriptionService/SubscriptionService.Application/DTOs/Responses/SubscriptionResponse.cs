using SubscriptionService.Domain.Enums;

namespace SubscriptionService.Application.DTOs.Responses;

public class SubscriptionResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }
    
    public SubscriptionStatus Status { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}