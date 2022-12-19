using Cancun.Booking.Application.Services;
using Cancun.Booking.Domain.Enums;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Cancun.Booking.SqlServer.Context;
using Cancun.Booking.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Services;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Ioc
{
    public class ConfigureServices
    {
        public static IServiceCollection Configure(LambdaServices lambdaServices)
        {
            return lambdaServices switch
            {
                LambdaServices.PlaceReservation => ConfigurePlaceReservationServices(),
                LambdaServices.CancelReservation => ConfigureCancelReservationServices(),
                LambdaServices.ModifyReservation => ConfigureModifyReservationService(),
                _ => throw new NotImplementedException(),
            };
        }

        #region Private methods


        private static IServiceCollection ConfigureModifyReservationService()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<IParameterService, ParameterService>();

            return ConfigureDataBase(services)
                .AddScoped<INotificatorService, NotificatorService>()
                .AddScoped<IPreBookingValidatorService, PreBookingValidatorService>()
                .AddScoped<IModifyReservationService, ModifyReservationService>()
                .AddScoped<IReservationRepository, ReservationRepository>();
        }

        private static IServiceCollection ConfigurePlaceReservationServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<IParameterService, ParameterService>();

            return ConfigureDataBase(services)
                .AddScoped<INotificatorService, NotificatorService>()
                .AddScoped<IPreBookingValidatorService, PreBookingValidatorService>()
                .AddScoped<IPlaceReservationService, PlaceReservationService>()
                .AddScoped<IReservationRepository, ReservationRepository>();
        }

        private static IServiceCollection ConfigureCancelReservationServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<IParameterService, ParameterService>();

            return ConfigureDataBase(services)
                .AddScoped<INotificatorService, NotificatorService>()
                .AddScoped<ICancelBookingService, CancelBookingService>()
                .AddScoped<IReservationRepository, ReservationRepository>();
        }

        private static IServiceCollection ConfigureDataBase(IServiceCollection services) =>
            services.AddDbContext<CancunDbContext>(options =>
            {
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CancunDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            });


        #endregion
    }
}