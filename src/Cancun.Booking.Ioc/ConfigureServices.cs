using Cancun.Booking.Application.Services;
using Cancun.Booking.Domain.Enums;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Cancun.Booking.MySql.Context;
using Cancun.Booking.MySql.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notification.Application.Services;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Ioc
{
    public class ConfigureServices
    {
        public static IServiceCollection Configure(LambdaServices lambdaServices)
        {
            IServiceCollection services = lambdaServices switch
            {
                LambdaServices.PlaceReservation => ConfigurePlaceReservationServices(),
                LambdaServices.CancelReservation => ConfigureCancelReservationServices(),
                LambdaServices.ModifyReservation => ConfigureModifyReservationServices(),
                LambdaServices.CheckAvailability => ConfigureCheckAvailabilityServices(),
                _ => throw new NotImplementedException(),
            };

            return AddCustomLogging(services);
        }

        #region Private methods



        private static IServiceCollection ConfigureCheckAvailabilityServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<IParameterService, ParameterService>();

            return ConfigureDataBase(services)
                .AddScoped<INotificatorService, NotificatorService>()
                .AddScoped<IRoomAvailabilityService, RoomAvailabilityService>()
                .AddScoped<IReservationRepository, ReservationRepository>();
        }

        private static IServiceCollection ConfigureModifyReservationServices()
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
                options.UseMySQL(Environment.GetEnvironmentVariable("DBCONNECTIONSTRING"));
            });

        private static IServiceCollection AddCustomLogging(IServiceCollection services) =>
            services.AddLogging((logging) =>
            {
                logging.AddLambdaLogger();
                logging.SetMinimumLevel(LogLevel.Debug);
            });

        #endregion
    }
}