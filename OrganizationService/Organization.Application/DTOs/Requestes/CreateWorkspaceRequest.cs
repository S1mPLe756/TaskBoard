using System.ComponentModel.DataAnnotations;

namespace Organization.Application.DTOs.Requestes;

public class CreateWorkspaceRequest
{
    [Required(ErrorMessage = "Имя обязательно")]
    public string Name { get; set; }
    
    [StringLength(maximumLength: 100, ErrorMessage = "Описание может быть только до 100 символов")]
    public string? Description { get; set; }
    
}