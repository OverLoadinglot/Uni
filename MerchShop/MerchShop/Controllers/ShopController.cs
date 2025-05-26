using MerchShop.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MerchShop.Controllers;

public class ShopController : Controller
{
    private readonly AppDBContext _context;

    public ShopController(AppDBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Shop()
    {
        var collections = await _context.Collections
            .Include(c => c.Products)
            .ToListAsync();
        return View(collections);
    }

    public async Task<IActionResult> ProductDetail(int id)
    {
        var product = await _context.Products
            .Include(p => p.Collection)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost]
    public IActionResult AddToCart(int productId)
    {
        return RedirectToAction("Shop");
    }
}