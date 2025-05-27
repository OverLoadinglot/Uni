using System.ComponentModel.DataAnnotations;

namespace MerchShop.Views.DTO;

public class MusicDTO
{
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Artists field is required.")]
    public string Artists { get; set; }
    
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Music link is required.")]
    [Url(ErrorMessage = "Invalid URL format.")]
    public string MusicLink { get; set; }
    
    [Required(ErrorMessage = "Image is required.")]
    public IFormFile Image { get; set; }
}