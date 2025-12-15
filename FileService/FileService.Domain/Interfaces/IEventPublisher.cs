using FileService.Domain.Events;

namespace FileService.Domain.Interfaces;

public interface IEventPublisher
{
    Task ProduceAsync(string topic, FileUploadEvent evt);
}