using MerchShop.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MerchShop.Controllers;

public class ProfileController : Controller
{
    private readonly AppDBContext _context;

    public ProfileController(AppDBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> UserProfile()
    {
        var userName = HttpContext.Session.GetString("Username");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Name == userName);

        return View(user);
    }
}