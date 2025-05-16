using E_commerce.Core.Interfaces;
using E_commerce.Core.Services;
using E_commerce.infrastructure.Data;
using E_commerce.infrastructure.Repositories;
using E_commerce.infrastructure.Services;
using Ecommerce.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;


namespace E_commerce.infrastructure
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection InfrastructureConfig(this IServiceCollection services, IConfiguration configuration)
        {
            // Add your infrastructure services
            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //redister services
            services.AddSingleton<IImageManagementService, ImageManagementService>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));



            return services;
        }
    }
}
