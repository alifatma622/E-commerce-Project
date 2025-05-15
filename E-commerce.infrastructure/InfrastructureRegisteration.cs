using E_commerce.Core.Interfaces;
using E_commerce.infrastructure.Data;
using E_commerce.infrastructure.Repositories;
using Ecommerce.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;
        }
    }
}
