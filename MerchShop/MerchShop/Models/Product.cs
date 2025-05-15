using System.ComponentModel.DataAnnotations;

namespace MerchShop.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }

    public double? Discount { get; set; }
}