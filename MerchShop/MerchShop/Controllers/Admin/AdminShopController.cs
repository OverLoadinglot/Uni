using MerchShop.Models;
using Microsoft.AspNetCore.Mvc;
using MerchShop.Models.Data;
using MerchShop.Views.DTO;
using Microsoft.EntityFrameworkCore;

namespace MerchShop.Controllers
{
    public class AdminShopController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminShopController(AppDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> AddCollection(CollectionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Shop", "Shop");
            }
            
            var extension = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains(extension))
            {
                return RedirectToAction("Shop", "Shop");
            }

            var fileName = Path.GetFileNameWithoutExtension(dto.Image.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var path = Path.Combine(_environment.WebRootPath, "img/Shop", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var collection = new Collection
            {
                Name = dto.Name,
                Image = "/img/Shop/" + fileName
            };

            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();

            return RedirectToAction("Shop", "Shop");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Shop", "Shop");
            }
            
            var extension = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains(extension))
            {
                return RedirectToAction("Shop", "Shop");
            }

            var fileName = Path.GetFileNameWithoutExtension(dto.Image.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var path = Path.Combine(_environment.WebRootPath, "img/Shop", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var product = new Product
            {
                Name = dto.Name,
                CollectionId = dto.CollectionId,
                Description = dto.Description,
                Price = dto.Price,
                Discount = dto.Discount,
                Stock = dto.Stock,
                Image = "/img/Shop/" + fileName
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Shop", "Shop");
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Forbid();
            }
            
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(product.Image) && product.Image.StartsWith("/img/Shop/"))
            {
                var imagePath = Path.Combine(_environment.WebRootPath, product.Image.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Shop", "Shop");
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Forbid();
            }

            var collection = await _context.Collections
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (collection == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(collection.Image) && collection.Image.StartsWith("/img/Shop/"))
            {
                var imagePath = Path.Combine(_environment.WebRootPath, collection.Image.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            foreach (var product in collection.Products)
            {
                if (!string.IsNullOrEmpty(product.Image) && product.Image.StartsWith("/img/Shop/"))
                {
                    var productImagePath = Path.Combine(_environment.WebRootPath, product.Image.TrimStart('/'));
                    if (System.IO.File.Exists(productImagePath))
                    {
                        System.IO.File.Delete(productImagePath);
                    }
                }
            }

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();

            return RedirectToAction("Shop", "Shop");
        }
    }
}