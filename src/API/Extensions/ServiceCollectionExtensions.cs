using Application.Interfaces;
using Application.Services;
using Infra;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCancunServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<CancunDbContext>(options =>
            {
                var builder = new MySqlConnectionStringBuilder
                {
                    Server = configuration["DB_SERVER"] ?? "localhost",
                    Database = configuration["DB_DATABASE"] ?? "cancun_db",
                    UserID = configuration["DB_USER"] ?? "root",
                    Password = configuration["DB_PASSWORD"] ?? "password"
                };
                
                options.UseMySQL(builder.ConnectionString);
            });

            // Add Application Services
            services.AddScoped<ReservationOrderService>();
            services.AddScoped<IRoomAvailabilityService, RoomAvailabilityService>();

            return services;
        }
    }
}
