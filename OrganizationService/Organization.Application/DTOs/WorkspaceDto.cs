namespace Organization.Application.DTOs;

public class WorkspaceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<WorkspaceMemberDto> Members { get; set; } = new();
}