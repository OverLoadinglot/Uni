using MerchShop.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MerchShop.Controllers;

public class MusicController : Controller
{
    private readonly AppDBContext _context;

    public MusicController(AppDBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Music()
    {
        var musics = await _context.Musics.ToListAsync();
        return View(musics);
    }
}