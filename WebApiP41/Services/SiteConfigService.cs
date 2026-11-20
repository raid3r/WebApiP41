using Microsoft.EntityFrameworkCore;
using WebApiP41.Models;

namespace WebApiP41.Services;

public class SiteConfigService(ApiDbContext dbContext, UploadImageService uploadImageService)
{

    /// <summary>
    /// Змінити фонове зображення
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public async Task ChangeBackgroundImageAsync(ImageFile image)
    {
        // Дістали з базі налаштування
        var siteConfig = await dbContext.SiteConfig
            .Include(x => x.BackgroundImage)
            .FirstOrDefaultAsync();
        // Якщо їх ще не було
        if (siteConfig == null)
        {
            // створили новий запис
            siteConfig = new SiteConfig();
            await dbContext.AddAsync(siteConfig);
        }

        // якщо було старе зображення
        if (siteConfig!.BackgroundImage != null) { 
            // видалити старе
            await uploadImageService.DeleteAsync(siteConfig.BackgroundImage);
            siteConfig.BackgroundImage = null;
        }

        // присвоїли зображення
        siteConfig!.BackgroundImage = image;
        // зберігли
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Отрммати посилання на фонове зображення
    /// </summary>
    /// <returns></returns>
    public async Task<string?> GetBackgroundImageUrlAsync()
    {
        // Дістали з базі налаштування
        var siteConfig = await dbContext.SiteConfig
            .Include(x => x.BackgroundImage)
            .FirstOrDefaultAsync();
        // Якщо їх ще не було
        if (siteConfig == null)
        {
            // створили новий запис
            siteConfig = new SiteConfig();
            await dbContext.AddAsync(siteConfig);
            await dbContext.SaveChangesAsync();
        }
        var filename = siteConfig!.BackgroundImage?.Filename;

        return !string.IsNullOrWhiteSpace(filename) ? $"/uploads/{filename}" : null;
    }



}
