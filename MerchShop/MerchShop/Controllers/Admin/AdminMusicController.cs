using MerchShop.Models;
using MerchShop.Models.Data;
using MerchShop.Views.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MerchShop.Controllers;

public class AdminMusicController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminMusicController(AppDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> AddMusic(MusicDTO dto)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Music", "Music");
            }
            
            var extension = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains(extension))
            {
                return RedirectToAction("Music", "Music");
            }

            var fileName = Path.GetFileNameWithoutExtension(dto.Image.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var path = Path.Combine(_environment.WebRootPath, "img/Music", fileName);

            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var music = new Music
            {
                Title = dto.Title,
                Artists = dto.Artists,
                Description = dto.Description,
                MusicLink = dto.MusicLink,
                Image = "/img/Music/" + fileName
            };

            _context.Musics.Add(music);
            await _context.SaveChangesAsync();

            return RedirectToAction("Music", "Music");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMusic(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Forbid();
            }
            
            var music = await _context.Musics.FirstOrDefaultAsync(t => t.Id == id);

            if (music == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(music.Image) && music.Image.StartsWith("/img/Music/"))
            {
                var imagePath = Path.Combine(_environment.WebRootPath, music.Image.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Musics.Remove(music);
            await _context.SaveChangesAsync();

            return RedirectToAction("Music", "Music");
        }
    }