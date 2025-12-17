using FileService.Application.DTOs;
using FileService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var fileId = await _fileService.UploadAsync(ms.ToArray(), file.FileName, file.ContentType);
        return Ok(new { Id = fileId, Url = await _fileService.GetFileUrl(fileId) });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Download(string id)
    {
        var bytes = await _fileService.DownloadAsync(id);
        return File(bytes, "application/octet-stream");
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _fileService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteFiles([FromBody] FilesDeleteRequest request)
    {
        await _fileService.DeleteFilesAsync(request.FileIds);
        return NoContent();
    }
}
