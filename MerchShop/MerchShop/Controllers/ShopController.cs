using MerchShop.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var products = await _context.Products.ToListAsync();
        return View(products);
    }
}