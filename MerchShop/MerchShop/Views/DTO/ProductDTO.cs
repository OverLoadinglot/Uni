namespace MerchShop.Views.DTO;

public class ProductDTO
{
    public string Name { get; set; }
    public int CollectionId { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public double? Discount { get; set; }
    public int Stock { get; set; }
    public IFormFile Image { get; set; }
}