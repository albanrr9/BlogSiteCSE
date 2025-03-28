using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogSite.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(AppDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var posts = _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        return View(posts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
