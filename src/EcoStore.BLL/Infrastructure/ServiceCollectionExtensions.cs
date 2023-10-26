using EcoStore.BLL.DTO;
using EcoStore.BLL.Services;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation;
using EcoStore.BLL.Validation.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace EcoStore.BLL.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IClock, Clock>();
        services.AddScoped<IValidator<CancelOrderByAdminDTO>, CancelOrderByAdminValidator>();
        services.AddScoped<IValidator<CancelOrderByUserDTO>, CancelOrderByUserValidator>();
        services.AddScoped<IValidator<CreateBrandDTO>, CreateBrandValidator>();
        services.AddScoped<IValidator<CreateCategoryDTO>, CreateCategoryValidator>();
        services.AddScoped<IValidator<CreateOrderDTO>, CreateOrderValidator>();
        services.AddScoped<IValidator<CreateProductDTO>, CreateProductValidator>();
        services.AddScoped<IValidator<UpdateBrandDTO>, UpdateBrandValidator>();
        services.AddScoped<IValidator<UpdateCategoryDTO>, UpdateCategoryValidator>();
        services.AddScoped<IValidator<UpdateOrderStatusDTO>, UpdateOrderStatusValidator>();
        services.AddScoped<IValidator<UpdateOrderTrackingNumberDTO>, UpdateOrderTrackingNumberValidator>();
        services.AddScoped<IValidator<UpdateProductDTO>, UpdateProductValidator>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}