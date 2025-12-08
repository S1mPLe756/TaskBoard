using System.Text.Json.Serialization;
using Organization.Domain.DTOs;
using Organization.Domain.Enums;

namespace Organization.Application.DTOs.Responses;

public class MemberUserResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserResponse User { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WorkspaceRole Role { get; set; }
}