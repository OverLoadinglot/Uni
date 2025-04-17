using System.Text.Json;
using MerchShop.Models;
using MerchShop.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace MerchShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDBContent _context;

        public AccountController(AppDBContent context)
        {
            _context = context;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(user);
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                ExportUsersToJson();
                
                var users = _context.Users.ToList();
                if (users.Count == 0)
                {
                    Console.WriteLine("Empty");
                }
                foreach (var u in users)
                {
                    Console.WriteLine($"ID: {u.Id}, Username: {u.Username}, Password: {u.Password}");
                }

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        private void ExportUsersToJson()
        {
            var users = _context.Users.ToList();

            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "users.json");

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            System.IO.File.WriteAllText(filePath, json);
        }
    }
}