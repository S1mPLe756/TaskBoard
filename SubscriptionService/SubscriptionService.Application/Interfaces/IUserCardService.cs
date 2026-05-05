using SubscriptionService.Application.DTOs.Responses;

namespace SubscriptionService.Application.Interfaces;

public interface IUserCardService
{
    Task<ConfirmationResponse> BindCardAsync(Guid userId);
    
    Task<List<UserCardResponse>> GetUserCardsAsync(Guid userId);
    
    Task DeleteCardAsync(Guid userId, Guid cardId);
    
    Task<UserCardResponse> SetDefaultCardAsync(Guid userId, Guid cardId);
    
    /// <summary>
    /// Обработка webhook от YooKassa
    /// </summary>
    Task ProcessYookassaWebhookAsync(Dictionary<string, object> payload);
}