using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult GioiThieu()
    {
        return View();
    }
    public IActionResult LienHe()
    {
        ViewData["Title"] = "Liên hệ";
        ViewData["PageType"] = "lienhe"; // để Layout phân biệt có/không banner
        return View();
    }
    public IActionResult BaoHanh()
    {
        ViewData["Title"] = "Bảo hành";
        ViewData["PageType"] = "baohanh"; // để Layout phân biệt có/không banner
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
