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
        services.AddScoped<IValidator<CreateOrderDTO>, CreateOrderValidator>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}