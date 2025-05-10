using System.Text.Json;
using Azure.Core;
using MerchShop.Models;
using MerchShop.Models.Data;
using MerchShop.Views.DBO;
using Microsoft.AspNetCore.Mvc;

namespace MerchShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDBContext _context;

        public AccountController(AppDBContext context)
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
        public async Task<IActionResult> Registration(RegistrationDBO userDBO)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Name == userDBO.Name);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(userDBO);
                }

                User user = new User()
                {
                    Name = userDBO.Name,
                    Password = userDBO.Password,
                    IdRole = 2
                };
                /*user.Name = userDBO.Name;
                user.Password = userDBO.Password;
                user.IdRole = 2;*/
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(userDBO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == email && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Name);
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
