using Application.Interfaces;
using Application.Services;
using Infra;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCancunServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<CancunDbContext>(options =>
            {
                var server = configuration["DB_SERVER"] ?? "localhost";
                var database = configuration["DB_DATABASE"] ?? "cancun_db";
                var user = configuration["DB_USER"] ?? "root";
                var password = configuration["DB_PASSWORD"] ?? "password";
                
                var connectionString = $"Server={server};Database={database};User={user};Password={password};";
                options.UseMySQL(connectionString);
            });

            // Add Application Services
            services.AddScoped<ReservationOrderService>();
            services.AddScoped<IRoomAvailabilityService, RoomAvailabilityService>();

            return services;
        }
    }
}
