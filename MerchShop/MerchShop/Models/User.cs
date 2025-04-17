using System.ComponentModel.DataAnnotations.Schema;

namespace MerchShop.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
    
    [NotMapped]
    public string ConfirmPassword { get; set; } = null!;
}