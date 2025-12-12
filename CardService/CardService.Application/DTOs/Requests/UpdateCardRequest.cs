using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CardService.Domain.Enum;

namespace CardService.Application.DTOs.Requests;

public class UpdateCardRequest
{
    [Required(ErrorMessage = "Id обязательно")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 100 символов")]
    public string Title { get; set; }
    [StringLength(256, MinimumLength = 0, ErrorMessage = "Описание должно быть от 0 до 256 символов")]
    public string? Description { get; set; }
    
    public List<UpdateCardLabelRequest> Labels { get; set; } = new();
    public UpdateCardChecklistRequest? Checklist { get; set; } = new();

    
    [Required(ErrorMessage = "Приоритет обязателен")]
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public CardPriority Priority { get; set; }
    
    [Required(ErrorMessage = "Дата окончания задачи обязательно")]
    public DateTime? DueDate { get; set; }

    
    [Required(ErrorMessage = "Id доски обязательно")]
    public Guid BoardId { get; set; }

    public List<Guid> AssigneeIds { get; set; } = new();
}