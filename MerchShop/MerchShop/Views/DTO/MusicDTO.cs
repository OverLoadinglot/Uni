using System.ComponentModel.DataAnnotations;

namespace MerchShop.Views.DTO;

public class MusicDTO
{
    public string Title { get; set; }

    public string Artists { get; set; }

    public string Description { get; set; }

    public string MusicLink { get; set; }

    public IFormFile Image { get; set; }
}