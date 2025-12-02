using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Organization.Application.Attributes;
using Organization.Domain.Enums;

namespace Organization.Application.DTOs.Requestes;

public class AddMemberRequest
{
    [Required(ErrorMessage = "Id пользователя обязательно")]
    [GuidNotEmpty(ErrorMessage = "Id пользователя не может быть пустым")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Роль обязательна")]
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public WorkspaceRole Role { get; set; }
}