using MerchShop.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Numerics;
using MerchShop.Models;

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
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            TempData["ErrorMessage"] = "Please, LogIn to add Item.";
            return RedirectToAction("Shop");
        }

        var product = await _context.Products.FindAsync(productId);
        if (product == null || product.Stock < quantity)
        {
            TempData["ErrorMessage"] = "Item not found or out of stock.";
            return RedirectToAction("ProductDetail", new { id = productId });
        }

        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = (int)userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            double totalPrice = product.Price;
            if (product.Discount != null)
            {
                totalPrice = product.Price * (1 - product.Discount.Value / 100);
            }
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = (decimal)totalPrice
            };
            _context.CartItems.Add(cartItem);
        }

        product.Stock -= quantity;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"{quantity} x {product.Name} has been successfully added to your cart!";
        return RedirectToAction("Cart");
    }
    
    public async Task<IActionResult> Cart()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return RedirectToAction("Login", "Authentication");
        }

        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = (int)userId };
        }

        return View(cart);
    }
    
    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        var cartItem = await _context.CartItems
            .Include(ci => ci.Cart)
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);

        if (cartItem == null)
        {
            TempData["ErrorMessage"] = "The cart element was not found.";
            return RedirectToAction("Cart");
        }

        cartItem.Product.Stock += cartItem.Quantity;

        _context.CartItems.Remove(cartItem);

        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.Id == cartItem.CartId);

        if (cart != null && !cart.CartItems.Any())
        {
            _context.Carts.Remove(cart);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"{cartItem.Product.Name} removed from cart.";
        return RedirectToAction("Cart");
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateCartQuantity(int cartItemId, int newQuantity)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        var cartItem = await _context.CartItems
            .Include(ci => ci.Cart)
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);

        if (cartItem == null)
        {
            TempData["ErrorMessage"] = "The cart element was not found.";
            return RedirectToAction("Cart");
        }

        if (newQuantity < 1)
        {
            return await RemoveFromCart(cartItemId);
        }

        if (cartItem.Product.Stock + cartItem.Quantity < newQuantity)
        {
            TempData["ErrorMessage"] = $"Not enough '{cartItem.Product.Name}' in stock.";
            return RedirectToAction("Cart");
        }

        cartItem.Product.Stock += (cartItem.Quantity - newQuantity);
        cartItem.Quantity = newQuantity;

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Updated Quantity of '{cartItem.Product.Name}'.";
        return RedirectToAction("Cart");
    }
}