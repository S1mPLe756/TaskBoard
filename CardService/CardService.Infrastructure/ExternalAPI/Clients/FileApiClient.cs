using System.Net;
using CardService.Domain.DTOs;
using CardService.Domain.Interfaces;
using ExceptionService;
using Microsoft.Extensions.Logging;
using Refit;

namespace CardService.Infrastructure.ExternalAPI.Clients;

public class FileApiClient(IFileApiRefitClient fileApiRefitClient, ILogger<FileApiClient> logger) : IFileApiClient
{
    public async Task<FileUploadResponse> UploadFileAsync(Stream file, String fileName, String contentType)
    {
        var streamPart = new StreamPart(file, fileName, contentType);
        try
        {
            return await fileApiRefitClient.UploadFile(streamPart);
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Error calling File API for file {FileName}", 
                fileName);
            throw new AppException("Файл не загрузился", HttpStatusCode.InternalServerError);
        }
    }
}