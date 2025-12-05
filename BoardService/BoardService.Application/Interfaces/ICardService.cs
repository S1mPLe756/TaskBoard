using BoardService.Application.DTOs;

namespace BoardService.Application.Interfaces;

public interface ICardService
{
    Task CreateCardPositionAsync(CreatedCardRequest dto);
}