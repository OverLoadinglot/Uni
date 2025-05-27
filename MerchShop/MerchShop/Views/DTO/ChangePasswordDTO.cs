using System.ComponentModel.DataAnnotations;

namespace MerchShop.Views.DTO;

public class ChangePasswordDTO
{
    [Required(ErrorMessage = "New password is required.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "New password must be at least 6 characters long.")]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Password confirmation is required.")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match.")]
    [Display(Name = "Confirm New Password")]
    public string ConfirmNewPassword { get; set; }
}