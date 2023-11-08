using System.Globalization;

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
            ? from.Value.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Utc)
            : null;
        DateTime? endDate = to.HasValue
            ? to.Value.ToDateTime(new TimeOnly(23, 59, 59), DateTimeKind.Utc)
            : null;
        var orders = (await _orderService.GetOrdersAsync(page, pageSize, userId, startDate, endDate))
            .Select(o => o.ToViewModel());
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
}