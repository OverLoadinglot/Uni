using System.ComponentModel.DataAnnotations;

namespace MerchShop.Models;

public class Music
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Artists { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Url]
    public string MusicLink { get; set; }

    public string Image { get; set; }
}