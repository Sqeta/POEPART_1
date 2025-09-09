using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using POEPART_1.Models;

namespace POEPART_1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult ViewApprove()
    {
        return View();
    }
    public IActionResult GenerateReport()
    {
        return View();
    }

    public IActionResult Lecture()
    {
        return View();
    }

    public IActionResult Programme_Coordinator()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Academic_Manager()
    {
        return View();
    }

    public IActionResult Home()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
