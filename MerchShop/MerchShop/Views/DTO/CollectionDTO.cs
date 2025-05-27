using System.ComponentModel.DataAnnotations;

namespace MerchShop.Views.DTO;

public class CollectionDTO
{
    [Required(ErrorMessage = "Collection name is required.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Image is required.")]
    public IFormFile Image { get; set; }
}