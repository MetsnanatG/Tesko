using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tesko.Data;
using Tesko.Models;
using Tesko.Models.ViewModels;

namespace Tesko.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TeskoDbContext _context;

    public HomeController(ILogger<HomeController> logger, TeskoDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel();

        // Stock Stats
        viewModel.AssetStocks = await _context.Assets
            .Select(a => new AssetStock { Name = a.Name, Total = a.TotalStock, Available = a.AvailableStock })
            .ToListAsync();

        // Top Items
        viewModel.TopItems = await _context.Requests
            .GroupBy(r => r.Asset.Name)
            .Select(g => new TopItem { Name = g.Key, RequestCount = g.Count() })
            .OrderByDescending(x => x.RequestCount)
            .Take(5)
            .ToListAsync();

        // Top Requesters
        viewModel.TopRequesters = await _context.Requests
            .GroupBy(r => r.User.Name)
            .Select(g => new TopRequester { Name = g.Key, RequestCount = g.Count() })
            .OrderByDescending(x => x.RequestCount)
            .Take(5)
            .ToListAsync();

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    // Helper to switch user
    public IActionResult SwitchUser(int id)
    {
        Response.Cookies.Append("CurrentUserId", id.ToString());
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardData()
    {
        var assetStocks = await _context.Assets
            .Select(a => new { a.Name, Total = a.TotalStock, Available = a.AvailableStock })
            .ToListAsync();

        var topItems = await _context.Requests
            .GroupBy(r => r.Asset.Name)
            .Select(g => new { Name = g.Key, RequestCount = g.Count() })
            .OrderByDescending(x => x.RequestCount)
            .Take(5)
            .ToListAsync();

        var topRequesters = await _context.Requests
            .GroupBy(r => r.User.Name)
            .Select(g => new { Name = g.Key, RequestCount = g.Count() })
            .OrderByDescending(x => x.RequestCount)
            .Take(5)
            .ToListAsync();

        return Json(new { assetStocks, topItems, topRequesters });
    }
}
