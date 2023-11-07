using System.Diagnostics;

using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IOrderService _orderService;

    public HomeController(ILogger<HomeController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    public IActionResult Index()
    {
        return Ok();
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
}