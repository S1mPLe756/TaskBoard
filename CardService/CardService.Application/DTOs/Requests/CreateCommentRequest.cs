using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CardService.Domain.Enum;

namespace CardService.Application.DTOs.Requests;

public class CreateCommentRequest
{
    [Required(ErrorMessage = "Текст обязателен")]
    [StringLength(256, MinimumLength = 3, ErrorMessage = "Текст должен быть от 3 до 256 символов")]
    public string Text { get; set; }

    [Required(ErrorMessage = "Id карточки обязательно")]
    public Guid CardId { get; set; }
    
    public Guid? AnsweredCommentId { get; set; }

}