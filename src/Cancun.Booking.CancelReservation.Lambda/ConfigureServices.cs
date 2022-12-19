using Cancun.Booking.Application.Services;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Cancun.Booking.SqlServer.Context;
using Cancun.Booking.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Services;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.CancelReservation.Lambda
{
    public static class ConfigureServices
    {
        public static IServiceCollection Configure()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<IParameterService, ParameterService>();

            services.AddDbContext<CancunDbContext>(options =>
            {
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CancunDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            });

            services.AddScoped<INotificatorService, NotificatorService>()
                .AddScoped<ICancelBookingService, CancelBookingService>()
                .AddScoped<IReservationRepository, ReservationRepository>();


            return services;
        }
    }
}
