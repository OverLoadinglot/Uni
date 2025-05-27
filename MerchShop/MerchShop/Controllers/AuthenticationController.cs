using MerchShop.Models;
using MerchShop.Models.Data;
using MerchShop.Views.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (!ModelState.IsValid) return View(userDto);
            
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == userDto.Email);
            
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This Email is already taken.");
                return View(userDto);
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            
            User user = new User()
            {
                Email = userDto.Email,
                Password = hashedPassword,
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
            User user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == userDto.Email);
            
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
                {
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("Role", user.Role.RoleName);
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.LoginError = "Invalid username or password.";
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Authentication");
        }
    }
}