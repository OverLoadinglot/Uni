using System.Text.Json;
using Azure.Core;
using MerchShop.Models;
using MerchShop.Models.Data;
using MerchShop.Views.DBO;
using Microsoft.AspNetCore.Mvc;

namespace MerchShop.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AppDBContext _context;

        public AuthenticationController(AppDBContext context)
        {
            _context = context;
        }
        
        public IActionResult Registration() => View();

        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationDTO userDto)
        {
            if (ModelState.IsValid) return View(userDto);
            
            var existingUser = _context.Users.FirstOrDefault(u => u.Name == userDto.Name);
            
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View(userDto);
            }

            User user = new User()
            {
                Name = userDto.Name,
                Password = userDto.Password,
                IdRole = 2
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginDTO userDto)
        {
            Console.WriteLine(_context.Users);
            User user = _context.Users.FirstOrDefault(u => u.Name == userDto.Name && u.Password == userDto.Password);
            Console.WriteLine(user);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Name);
                Console.WriteLine($"Роль: {user.Role}");
                //HttpContext.Session.SetString("Role", user.Role);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.LoginError = "Invalid username or password.";
            return View();
        }
    }
}