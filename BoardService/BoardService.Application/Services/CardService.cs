using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.Interfaces;
using BoardService.Domain.Entities;
using BoardService.Domain.Interfaces;

namespace BoardService.Application.Services;

public class CardService(ICardRepository repository, IMapper mapper) : ICardService
{

    public async Task CreateCardPositionAsync(CreatedCardRequest dto)
    {
        var card = mapper.Map<CardPosition>(dto);

        card.Position = await repository.CountAsync(dto.ColumnId);
        
        await repository.AddCardPositionAsync(card);
    }
}