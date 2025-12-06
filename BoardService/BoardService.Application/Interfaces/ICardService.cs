using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;

namespace BoardService.Application.Interfaces;

public interface ICardService
{
    Task CreateCardPositionAsync(CreatedCardRequest dto);
}