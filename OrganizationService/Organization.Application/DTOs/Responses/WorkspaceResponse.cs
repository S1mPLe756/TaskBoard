namespace Organization.Application.DTOs.Responses;

public class WorkspaceResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<WorkspaceMemberResponse> Members { get; set; } = new();
}