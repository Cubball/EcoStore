using EcoStore.DAL.Files;
using EcoStore.DAL.Files.Interfaces;
using EcoStore.DAL.Repositories;
using EcoStore.DAL.Repositories.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace EcoStore.DAL.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, string imagesPath)
    {
        services.AddScoped<IFileManager, FileManager>(services => new FileManager(imagesPath));
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        return services;
    }
}