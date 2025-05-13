using MerchShop.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace MerchShop.Controllers;

public class AdminController : Controller
{
    private readonly AppDBContext _context;

    public AdminController(AppDBContext context)
    {
        _context = context;
    }

    public IActionResult AdminPanel()
    {
        var users = _context.Users.ToList();
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        return RedirectToAction("AdminPanel");
    }
}
