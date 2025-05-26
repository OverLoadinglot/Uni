using MerchShop.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MerchShop.Controllers;

public class AdminController : Controller
{
    private readonly AppDBContext _context;

    public AdminController(AppDBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AdminPanel()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .ToListAsync();

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
