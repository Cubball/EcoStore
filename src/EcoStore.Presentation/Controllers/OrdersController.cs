using System.Globalization;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    private readonly UserManager<AppUser> _userManager;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;
    private readonly string _imagePath;

    public OrdersController(
            IOrderService orderService,
            UserManager<AppUser> userManager,
            IConfiguration configuration)
    {
        _orderService = orderService;
        _userManager = userManager;
        _imagePath = configuration["Path:Images"]!;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
    }

    public async Task<IActionResult> All(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null)
    {
        var userId = _userManager.GetUserId(User);
        if (page < 1)
        {
            page = _defaultPageNumber;
        }

        if (pageSize < 1)
        {
            pageSize = _defaultPageSize;
        }

        DateTime? startDate = from.HasValue
            ? from.Value.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Local).ToUniversalTime()
            : null;
        DateTime? endDate = to.HasValue
            ? to.Value.ToDateTime(new TimeOnly(23, 59, 59), DateTimeKind.Local).ToUniversalTime()
            : null;
        var orders = (await _orderService.GetOrdersAsync(page, pageSize, userId, startDate, endDate))
            .Select(o => o.ToViewModel())
            .ToList();
        foreach (var order in orders)
        {
            UpdateOrdersTime(order);
        }

        var ordersCount = await _orderService.GetOrderCountAsync(userId, startDate, endDate);
        var ordersListViewModel = new OrdersListViewModel
        {
            PageInfo = new PageInfoViewModel
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = ordersCount,
            },
            Orders = orders,
        };
        return View(ordersListViewModel);
    }

    // TODO: catch?
    public async Task<IActionResult> Details(int id)
    {
        var userId = _userManager.GetUserId(User);
        var order = await _orderService.GetOrderAsync(id);
        if (userId != order.User.Id)
        {
            return RedirectToAction("All");
        }

        var orderViewModel = order.ToViewModel();
        UpdateOrdersTime(orderViewModel);
        UpdateOrderedProductsImagePaths(orderViewModel);
        return View(orderViewModel);
    }

    // TODO: catch
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = _userManager.GetUserId(User);
        var order = await _orderService.GetOrderAsync(id);
        return userId == order.User.Id ? View(id) : RedirectToAction("All");
    }

    // TODO: catch
    public async Task<IActionResult> CancelConfirmed(int id)
    {
        var userId = _userManager.GetUserId(User);
        var order = await _orderService.GetOrderAsync(id);
        if (userId != order.User.Id)
        {
            return RedirectToAction("All");
        }

        var cancelOrderDTO = new CancelOrderByUserDTO { Id = id, };
        await _orderService.CancelOrderByUserAsync(cancelOrderDTO);
        return RedirectToAction("Details", new { id });
    }

    private void UpdateOrderedProductsImagePaths(OrderViewModel orderViewModel)
    {
        foreach (var orderedProduct in orderViewModel.OrderedProducts)
        {
            orderedProduct.Product!.ImagePath = Path.Combine(_imagePath, orderedProduct.Product.ImagePath);
        }
    }

    private static void UpdateOrdersTime(OrderViewModel orderViewModel)
    {
        orderViewModel.OrderDate = orderViewModel.OrderDate.ToLocalTime();
        orderViewModel.StatusChangedDate = orderViewModel.StatusChangedDate.ToLocalTime();
    }
}