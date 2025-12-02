using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Organization.Application.Attributes;
using Organization.Domain.Enums;

namespace Organization.Application.DTOs.Requestes;

public class CreateInvitationRequest
{
    [Required(ErrorMessage = "WorkspaceId обязателен")]
    [GuidNotEmpty(ErrorMessage = "WorkspaceId не может быть пустым")]
    public Guid WorkspaceId { get; set; }

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Неверный формат email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Role обязателен")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WorkspaceRole Role { get; set; }
}