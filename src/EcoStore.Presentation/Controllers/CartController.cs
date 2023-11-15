using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.DAL.Entities;
using EcoStore.Presentation.Extensions;
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

    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly UserManager<AppUser> _userManager;
    private readonly string _imagePath;

    public CartController(
            IProductService productService,
            IOrderService orderService,
            UserManager<AppUser> userManager,
            IConfiguration configuration)
    {
        _productService = productService;
        _orderService = orderService;
        _userManager = userManager;
        _imagePath = configuration["Path:Images"]!;
    }

    public async Task<IActionResult> Index()
    {
        return View(await GetCart());
    }

    public async Task<IActionResult> Checkout()
    {
        return View(new CreateOrderViewModel { Cart = await GetCart() });
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CreateOrderViewModel createOrder)
    {
        createOrder.Cart = await GetCart();
        var userId = _userManager.GetUserId(User);
        var createOrderDTO = createOrder.ToDTO();
        createOrderDTO.UserId = userId!;
        try
        {
            var orderId = await _orderService.CreateOrderAsync(createOrderDTO);
            ClearCartCookie();
            return View("CheckoutSuccess", orderId);
        }
        catch (ValidationException e)
        {
            e.AddErrorsToModelState(ModelState);
            return View(createOrder);
        }
    }

    public IActionResult CheckoutSuccess()
    {
        return View();
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
            var product = (await _productService.GetProductByIdAsync(entry.Key)).ToViewModel();
            product.ImagePath = Path.Combine(_imagePath, product.ImagePath);
            cartItems.Add(new CartItemViewModel
            {
                Product = product,
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

    private void ClearCartCookie()
    {
        var cartCookieKey = GetCartCookieKey();
        var cartCookie = Request.Cookies[cartCookieKey];
        if (cartCookie is not null)
        {
            Response.Cookies.Delete(cartCookieKey);
        }
    }
}