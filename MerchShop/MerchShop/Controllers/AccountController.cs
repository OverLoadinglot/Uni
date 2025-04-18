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
        
        public IActionResult Login()
        {
            return View();
        }
    
        public IActionResult UserList()
        {
            var users = _context.Users.ToList();
            return View(users);
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

                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == email && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.LoginError = "Invalid username or password.";
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("UserList");
        }

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    }
}
