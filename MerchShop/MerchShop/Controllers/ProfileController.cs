using MerchShop.Models.Data;
using MerchShop.Views.DTO;
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

    [HttpGet]
    public async Task<IActionResult> UserProfile()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "Authentication");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == userEmail);

        if (user == null)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Authentication");
        }

        ViewBag.UserEmail = user.Email;
        return View(new ChangePasswordDTO());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserProfile(ChangePasswordDTO model)
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "Authentication");
        }

        var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

        if (userToUpdate == null)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Authentication");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.UserEmail = userToUpdate.Email;
            return View(model);
        }

        userToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("UserProfile");
    }
}