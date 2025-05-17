using Microsoft.OpenApi.Models;
using E_commerce.infrastructure;
using AutoMapper;
using E_commerce.API.Middlewares;

namespace E_commerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMemoryCache();

            // Configure Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-commerce API", Version = "v1" });
            });

            // Add infrastructure services (includes AutoMapper configuration)
            builder.Services.InfrastructureConfig(builder.Configuration);
            // Add AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-commerce API v1"));
            }

            app.UseMiddleware<ExpectionMiddleware>();

            app.UseHttpsRedirection();

            app.UseStatusCodePagesWithRedirects("/Error/{0}"); // Redirect to ErrorController for status codes

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
