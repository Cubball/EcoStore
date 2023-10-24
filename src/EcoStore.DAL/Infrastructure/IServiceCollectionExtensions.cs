using EcoStore.DAL.Repositories;
using EcoStore.DAL.Repositories.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace EcoStore.DAL.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}