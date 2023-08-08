using Microsoft.AspNetCore.Mvc;
using TShared.Azure.Storage.Abstraction;

namespace AzureLearning.Api.Controllers;

[ApiController]
[Route("[controller]s")]
public class UserController : ControllerBase
{
    private readonly IAzureBlobService _azureBlobService;

    public UserController(IAzureBlobService azureBlobService)
    {
        _azureBlobService = azureBlobService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] UploadFile file)
    {
        var stream = file.File.OpenReadStream();
        await _azureBlobService.UploadFileAsync(file.ContainerName, file.File.FileName, stream);
        var uri = _azureBlobService.GetSasFileUrl(file.ContainerName, file.File.FileName);
        return Ok(uri);
    }
}

public class UploadFile
{
    public IFormFile? File { get; set; }
    public string? ContainerName { get; set; }
}