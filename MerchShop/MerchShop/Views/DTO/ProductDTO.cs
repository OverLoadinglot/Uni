using System.ComponentModel.DataAnnotations;

namespace MerchShop.Views.DTO;

public class ProductDTO
{
    [Required(ErrorMessage = "Product name is required.")]
    public string Name { get; set; }
    public int CollectionId { get; set; }
    
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(300, ErrorMessage = "Description cannot be longer than 300 characters.")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
    public double Price { get; set; }
    
    [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
    public double? Discount { get; set; }
    [Required(ErrorMessage = "Stock is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or more.")]
    public int Stock { get; set; }
    [Required(ErrorMessage = "Image is required.")]
    public IFormFile Image { get; set; }
}