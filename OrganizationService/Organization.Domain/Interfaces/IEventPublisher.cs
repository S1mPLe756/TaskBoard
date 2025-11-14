using Organization.Domain.Events;

namespace Organization.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishInvitationSendAsync(InvitationSendEvent evt);
}