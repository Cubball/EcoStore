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

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetOrdersByUserIdAsync("db0865fe-f30f-4c89-b8e7-0d72fa90e7a5", 2, 1);
        return Ok(orders);
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