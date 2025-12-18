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

    public async Task<Guid> SendAsync(NotificationRequest request, CancellationToken ct = default)
    {
        var notif = _mapper.Map<Notification>(request);

        await _repo.AddAsync(notif);

        try
        {
            ct.ThrowIfCancellationRequested();

            switch (request.Type)
            {
                case NotificationType.Email:
                    await _email.SendAsync(request.To, request.Title, request.Message, ct);
                    break;

            }

            notif.Sent = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            notif.Sent = false;
            notif.Error = ex.Message;
        }

        await _repo.UpdateAsync(notif);

        return notif.Id;
    }

    public async Task<Guid> SendAsync(NotificationBulkRequest request, CancellationToken ct = default)
    {
        if (!request.Emails.Any())
            throw new ArgumentException("Recipients list is empty");

        var notifications = request.Emails
            .Distinct()
            .Select(email => new Notification
            {
                To = email,
                Title = request.Title,
                Message = request.Message,
                Type = request.Type,
                Sent = false
            })
            .ToList();

        await _repo.AddRangeAsync(notifications);

        try
        {
            ct.ThrowIfCancellationRequested();

            switch (request.Type)
            {
                case NotificationType.Email:
                    await _email.SendBulkAsync(
                        notifications.Select(n => n.To),
                        request.Title,
                        request.Message,
                        ct
                    );
                    break;
            }

            notifications.ForEach(n => n.Sent = true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            notifications.ForEach(n =>
            {
                n.Sent = false;
                n.Error = ex.Message;
            });
        }

        await _repo.UpdateRangeAsync(notifications);

        return notifications.First().Id;
    }
}