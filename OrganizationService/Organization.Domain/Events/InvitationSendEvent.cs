namespace Organization.Domain.Events;

public class InvitationSendEvent
{
    public string Email { get; set; } = null!;
    public string OrganizationName { get; set; } = null!;
    public Guid InvitationId { get; set; }
}