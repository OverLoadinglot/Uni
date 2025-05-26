using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MerchShop.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [ForeignKey("Collection")]
    public int CollectionId { get; set; }

    public Collection Collection { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public double Price { get; set; }

    public double? Discount { get; set; }

    [Required]
    public int Stock { get; set; }

    [Required]
    public string Image { get; set; }
}