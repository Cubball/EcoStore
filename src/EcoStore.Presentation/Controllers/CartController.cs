using EcoStore.BLL.Services.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace EcoStore.Presentation.Controllers;

[Authorize]
public class CartController : Controller
{
    private const string CarCookieKeyPrefix = "cart_for_";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly UserManager<AppUser> _userManager;

    public CartController(
            IHttpContextAccessor httpContextAccessor,
            IProductService productService,
            IOrderService orderService,
            UserManager<AppUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _productService = productService;
        _orderService = orderService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        return View(await GetCart());
    }

    private async Task<CartViewModel> GetCart()
    {
        var cartCookieKey = GetCartCookieKey();
        var cartCookie = Request.Cookies[cartCookieKey];
        var cart = new CartViewModel();
        if (cartCookie is null)
        {
            return cart;
        }

        var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartCookie);
        if (keyValuePairs is null || keyValuePairs.Count == 0)
        {
            return cart;
        }

        var cartItems = new List<CartItemViewModel>();
        foreach (var entry in keyValuePairs)
        {
            cartItems.Add(new CartItemViewModel
            {
                Product = (await _productService.GetProductByIdAsync(entry.Key)).ToViewModel(),
                Quantity = entry.Value,
            });
        }

        cart.CartItems = cartItems;
        return cart;
    }

    private string GetCartCookieKey()
    {
        var userId = _userManager.GetUserId(User);
        return CarCookieKeyPrefix + userId;
    }
}