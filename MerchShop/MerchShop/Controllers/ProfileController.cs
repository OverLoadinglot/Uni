using MerchShop.Views.DBO;
using Microsoft.AspNetCore.Mvc;

namespace MerchShop.Controllers;

public class ProfileController : Controller
{
    public IActionResult UserProfile() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]

    public IActionResult UserProfile(LoginDTO userDTO)
    {
        return View();
    }
}