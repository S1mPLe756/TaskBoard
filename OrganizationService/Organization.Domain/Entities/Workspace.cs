namespace Organization.Domain.Entities;

public class Workspace
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; } // UserId владельца
    public List<WorkspaceMember> Members { get; set; } = new();
}
