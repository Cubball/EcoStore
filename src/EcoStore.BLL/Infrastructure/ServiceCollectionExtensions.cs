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
        services.AddScoped<IGuidProvider, GuidProvider>();
        services.AddScoped<IValidator<AdminRegisterDTO>, AdminRegisterValidator>();
        services.AddScoped<IValidator<CancelOrderByAdminDTO>, CancelOrderByAdminValidator>();
        services.AddScoped<IValidator<CancelOrderByUserDTO>, CancelOrderByUserValidator>();
        services.AddScoped<IValidator<CreateBrandDTO>, CreateBrandValidator>();
        services.AddScoped<IValidator<CreateCategoryDTO>, CreateCategoryValidator>();
        services.AddScoped<IValidator<CreateOrderDTO>, CreateOrderValidator>();
        services.AddScoped<IValidator<CreateProductDTO>, CreateProductValidator>();
        services.AddScoped<IValidator<UpdateAppUserDTO>, UpdateAppUserValidator>();
        services.AddScoped<IValidator<UpdateBrandDTO>, UpdateBrandValidator>();
        services.AddScoped<IValidator<UpdateCategoryDTO>, UpdateCategoryValidator>();
        services.AddScoped<IValidator<UpdateOrderStatusDTO>, UpdateOrderStatusValidator>();
        services.AddScoped<IValidator<UpdateOrderTrackingNumberDTO>, UpdateOrderTrackingNumberValidator>();
        services.AddScoped<IValidator<UpdateProductDTO>, UpdateProductValidator>();
        services.AddScoped<IValidator<UserChangePasswordDTO>, UserChangePasswordValidator>();
        services.AddScoped<IValidator<UserRegisterDTO>, UserRegisterValidator>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IHtmlWriter, HtmlWriter>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}