using Microsoft.AspNetCore.Mvc;

namespace WebApiP41.Controllers;

[Route("upload")]
[ApiController]
public class UploadController(IWebHostEnvironment env) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        var uploadsFolder = Path.Combine(env.WebRootPath, "uploads");

        // dev.jpg
        var originalFileName = Path.GetFileName(file.FileName);

        // .jpg
        var fileExtension = Path.GetExtension(originalFileName);

        // 12345-12345-12345-12345-12345.jpg
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        return Ok(new { FileName = uniqueFileName });
    }


}
