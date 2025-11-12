using AuthService.Domain.Events;

namespace AuthService.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishUserRegisteredAsync(UserRegisteredEvent evt);
}