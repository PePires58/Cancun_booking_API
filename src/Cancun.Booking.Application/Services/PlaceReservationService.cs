using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Application.Services
{
    public class PlaceReservationService : BaseService, IPlaceReservationService
    {
        #region Properties
        IPreBookingValidatorService IPreBookingValidatorService { get; set; }
        IReservationRepository IReservationRepository { get; set; }
        ILogger<PlaceReservationService> ILogger { get; set; }
        #endregion

        #region Constructor
        public PlaceReservationService(INotificatorService INotificatorService,
            IPreBookingValidatorService IPreBookingValidatorService,
            IReservationRepository IReservationRepository,
            ILogger<PlaceReservationService> ILogger) : base(INotificatorService)
        {
            this.IPreBookingValidatorService = IPreBookingValidatorService;
            this.IReservationRepository = IReservationRepository;
            this.ILogger = ILogger;
        }

        public void PlaceReservation(ReservationOrder reservationOrder)
        {
            ILogger.LogInformation("Starting place reservation function");

            ILogger.LogInformation("Starting validation of reservation order", reservationOrder);
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);
            if (INotificatorService.HasNotification)
            {
                ILogger.LogInformation("Validation completed, the object is not valid");
                return;
            }

            ILogger.LogInformation("Starting check availability for the reservation");
            if (IReservationRepository.CheckAvailability(reservationOrder))
            {
                ILogger.LogInformation("Starting insert and save on database");
                IReservationRepository.Insert(reservationOrder);
                IReservationRepository.Save();
                ILogger.LogInformation("Process of place reservation completed");
            }
            else
            {
                ILogger.LogInformation("Check Availability complete, the room is not available for the date");
                HandleNotification($"The room {reservationOrder.RoomId} is not available for this date");
            }
        }
        #endregion

    }
}
