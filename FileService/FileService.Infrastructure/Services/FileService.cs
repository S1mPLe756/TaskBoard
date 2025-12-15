using FileService.Domain.Interfaces;
using File = FileService.Domain.Entities.File;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;


namespace FileService.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IGridFSBucket _gridFS;
    private readonly IEventPublisher _kafkaProducer;

    public FileService(IMongoDatabase database, IEventPublisher kafkaProducer)
    {
        _gridFS = new GridFSBucket(database);
        _kafkaProducer = kafkaProducer;
    }

    public async Task<string> UploadAsync(byte[] content, string fileName, string contentType)
    {
        var options = new GridFSUploadOptions
        {
            Metadata = new MongoDB.Bson.BsonDocument
            {
                { "ContentType", contentType },
                { "UploadedAt", DateTime.UtcNow }
            }
        };
        var id = await _gridFS.UploadFromBytesAsync(fileName, content, options);

        await _kafkaProducer.ProduceAsync("file_uploaded", new () { FileId = id.ToString(), FileName = fileName });

        return id.ToString();
    }

    public async Task<byte[]> DownloadAsync(string fileId)
    {
        return await _gridFS.DownloadAsBytesAsync(new MongoDB.Bson.ObjectId(fileId));
    }

    public async Task DeleteAsync(string fileId)
    {
        await _gridFS.DeleteAsync(new MongoDB.Bson.ObjectId(fileId.ToString()));
        await _kafkaProducer.ProduceAsync("file_deleted", new () { FileId = fileId });
    }

    public async Task<IEnumerable<File>> ListAsync()
    {
        var files = await _gridFS.Find(FilterDefinition<GridFSFileInfo>.Empty).ToListAsync();
        return files.Select(f => new File
        {
            Id = f.Id.ToString(),
            FileName = f.Filename,
            ContentType = f.Metadata.GetValue("ContentType").AsString,
            Size = f.Length,
            UploadedAt = f.UploadDateTime
        });
    }

    public Task<string> GetFileUrl(string fileId)
    {
        return Task.FromResult($"/api/v1/files/{fileId}");
    }
}