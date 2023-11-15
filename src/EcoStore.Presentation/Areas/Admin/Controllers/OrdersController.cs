using System.Globalization;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Areas.Admin.Mapping;
using EcoStore.Presentation.Areas.Admin.ViewModels;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;
    private readonly string _imagePath;

    public OrdersController(
            IOrderService orderService,
            IUserService userService,
            IConfiguration configuration)
    {
        _orderService = orderService;
        _userService = userService;
        _imagePath = configuration["Path:Images"]!;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
    }

    public async Task<IActionResult> All(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
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
        var orders = (await _orderService.GetOrdersAsync(
                    pageNumber: page,
                    pageSize: pageSize,
                    startDate: startDate,
                    endDate: endDate))
            .Select(o => o.ToViewModel())
            .ToList();
        var ordersCount = await _orderService.GetOrderCountAsync(startDate: startDate, endDate: endDate);
        var ordersListViewModel = new OrdersListViewModel
        {
            PageInfo = new PageInfoViewModel
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = ordersCount,
            },
            Orders = orders,
            From = from?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            To = to?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        };
        return View(ordersListViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = (await _orderService.GetOrderAsync(id)).ToViewModel();
        foreach (var orderedProduct in order.OrderedProducts)
        {
            orderedProduct.Product!.ImagePath = Path.Combine(_imagePath, orderedProduct.Product.ImagePath);
        }
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(int id, NewOrderStatusViewModel orderStatus)
    {
        var updateDTO = new UpdateOrderStatusDTO
        {
            Id = id,
            OrderStatus = orderStatus.ToDTO(),
        };
        await _orderService.UpdateOrderStatusAsync(updateDTO);
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTrackingNumber(int id, string trackingNumber)
    {
        var updateDTO = new UpdateOrderTrackingNumberDTO
        {
            Id = id,
            TrackingNumber = trackingNumber,
        };
        await _orderService.UpdateOrderTrackingNumberAsync(updateDTO);
        return RedirectToAction(nameof(Details), new { id });
    }

    public IActionResult Cancel(int id)
    {
        return View(id);
    }

    public async Task<IActionResult> CancelConfirmed(int id)
    {
        await _orderService.CancelOrderByAdminAsync(new CancelOrderByAdminDTO { Id = id });
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = (await _orderService.GetOrderAsync(id)).ToViewModel();
        return View(product);
    }

    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _orderService.DeleteOrderAsync(id);
        return RedirectToAction(nameof(All));
    }

    public new async Task<IActionResult> User(string id,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
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
        var orders = (await _orderService.GetOrdersAsync(
                    pageNumber: page,
                    pageSize: pageSize,
                    userId: id,
                    startDate: startDate,
                    endDate: endDate))
            .Select(o => o.ToViewModel())
            .ToList();
        var ordersCount = await _orderService.GetOrderCountAsync(userId: id, startDate: startDate, endDate: endDate);
        var ordersListViewModel = new AppUsersOrdersListViewModel
        {
            PageInfo = new PageInfoViewModel
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = ordersCount,
            },
            Orders = orders,
            User = (await _userService.GetUserByIdAsync(id)).ToViewModel(),
        };
        return View(ordersListViewModel);
    }
}