using System.ComponentModel.DataAnnotations;

namespace MerchShop.Models;

public class Collection
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Image { get; set; }

    public List<Product> Products { get; set; }
}