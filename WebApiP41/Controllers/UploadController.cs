using Microsoft.AspNetCore.Mvc;
using WebApiP41.Services;
using WebApiP41.Models.DTO;
using Microsoft.Identity.Client.NativeInterop;

namespace WebApiP41.Controllers;

[Route("upload")]
[ApiController]
public class UploadController(
    UploadImageService uploadImageService,
    SiteConfigService siteConfigService
    ) : ControllerBase
{
    [HttpPost("background")]
    public async Task<IActionResult> UploadBackgroundImage([FromForm] IFormFile file)
    {
        try
        {
            var image = await uploadImageService.UploadAsync(file);
            await siteConfigService.ChangeBackgroundImageAsync(image);

            return Ok(new OperationResult
            {
                Success = true,
                ErrorMessage = null
            });
        } catch (Exception ex) {
        
            return BadRequest(new OperationResult
            {
                Success = false, 
                ErrorMessage = ex.Message
            });
        }
    }

    [HttpGet("background")]
    public async Task<IActionResult> GetBackgroundImage()
    {
        return Ok(new
        {
            ImageUrl = await siteConfigService.GetBackgroundImageUrlAsync()
        });
    }
}

/*
 

    Веб Сторінка в браузері
           завантажили файл на сервер - сервер повернув назву файлу {filename: ""}
           встановили атрибут стиль background-image: url(/uploads/12345-12345-...jpg)
           варіант 1. зберігти в localstorage
                   - може зникнути якшо користвувач перейде на інший бразузер або чистить localstorage
                   - інші користувачі не отримають цих даних
           варіант 2. 
                   - окремий метод на бекенді який віддає посилання на файл
                     - дата та час нендійні - їх може змінити будь яка файлова опрерація
                     - 
                   
             



    оновили сторінку
           фону немає
            
           
    





 */ 