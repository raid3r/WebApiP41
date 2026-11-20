namespace WebApiP41.Models;

public class ImageFile
{
    public int Id { get; set; }
    public string OriginalFilename { get; set; }
    public string Filename { get; set; }
    public DateTime CreatedAt { get; set; }
}
