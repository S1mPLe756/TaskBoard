using AutoMapper;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Interfaces;

namespace NotificationService.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private readonly IEmailSender _email;
    private readonly IMapper _mapper;
    
    public NotificationService(
        INotificationRepository repo,
        IEmailSender email,
        IMapper mapper)
    {
        _repo = repo;
        _email = email;
        _mapper = mapper;
    }

    public async Task<Guid> SendAsync(NotificationRequest request)
    {
        var notif = _mapper.Map<Notification>(request);

        await _repo.AddAsync(notif);

        try
        {
            switch (request.Type)
            {
                case NotificationType.Email:
                    await _email.SendAsync(request.To, request.Title, request.Message);
                    break;

            }

            notif.Sent = true;
        }
        catch (Exception ex)
        {
            notif.Sent = false;
            notif.Error = ex.Message;
        }

        await _repo.UpdateAsync(notif);

        return notif.Id;
    }
}