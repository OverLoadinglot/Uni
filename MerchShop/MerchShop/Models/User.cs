using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MerchShop.Models
{
    public class User()
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        public int IdRole { get; set; }
        
        [ForeignKey("IdRole")]
        public Role Role { get; set; }
    }
}