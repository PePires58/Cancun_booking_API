using Cancun.Booking.Application.Validators;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Enums;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Application.Services
{
    public class CancelBookingService : BaseService, ICancelBookingService
    {
        #region Properties
        IReservationRepository IReservationRepository { get; set; }
        ILogger<CancelBookingService> ILogger { get; set; }
        #endregion
        public CancelBookingService(INotificatorService INotificatorService, 
            IReservationRepository IReservationRepository,
            ILogger<CancelBookingService> ILogger)
            : base(INotificatorService)
        {
            this.IReservationRepository = IReservationRepository;
            this.ILogger = ILogger;
        }

        public void CancelBooking(CancelReservationOrder cancelReservationOrder)
        {
            ILogger.LogWarning("Starting cancel booking service");

            ILogger.LogInformation("Starting validation of entry object", cancelReservationOrder);
            if (ObjectIsValid(new CancelReservationOrderValidators(), cancelReservationOrder))
            {
                ILogger.LogInformation("Check if reservation exists");
                if (IReservationRepository.Any(c => c.Id == cancelReservationOrder.ReservationId))
                {
                    ILogger.LogInformation("Check if reservation belongs to the customer");

                    if (IReservationRepository.Any(c => c.Id == cancelReservationOrder.ReservationId &&
                        c.CustomerEmail == cancelReservationOrder.CustomerEmail))
                    {
                        ILogger.LogInformation("Check if reservation is reserved");

                        if (IReservationRepository.Any(c => c.Id == cancelReservationOrder.ReservationId &&
                        c.Status == ReservationOrderStatus.Reserved))
                        {
                            ILogger.LogInformation("Starting cancel proccess on db");

                            ReservationOrder reservationOrderDb = IReservationRepository.GetById(cancelReservationOrder.ReservationId);
                            reservationOrderDb.Status = ReservationOrderStatus.Canceled;
                            IReservationRepository.Update(reservationOrderDb);
                            IReservationRepository.Save();

                            ILogger.LogInformation("Cancel proccess completed");
                        }
                        else
                            HandleNotification("You cannot cancel this reservation because it's already cancelled");
                    }
                    else
                        HandleNotification("You cannot cancel this reservation because it's not belongs to you");
                }
                else
                    HandleNotification("Reservation does not exists");
            }
        }
    }
}
