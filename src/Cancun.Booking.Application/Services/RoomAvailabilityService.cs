using Cancun.Booking.Application.Validators;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Application.Services
{
    public class RoomAvailabilityService : BaseService, IRoomAvailabilityService
    {
        #region Properties
        IReservationRepository IReservationRepository { get; set; }
        ILogger<RoomAvailabilityService> ILogger { get; set; }
        #endregion

        #region Constructor
        public RoomAvailabilityService(INotificatorService INotificatorService, 
            IReservationRepository IReservationRepository,
            ILogger<RoomAvailabilityService> ILogger)
            : base(INotificatorService)
        {
            this.IReservationRepository = IReservationRepository;
            this.ILogger = ILogger;

        }
        #endregion

        public bool CheckRoomAvailability(ReservationOrder reservationOrder)
        {
            ILogger.LogInformation("Starting process of Check Room Availability");

            ILogger.LogInformation("Start validation of entry object of reservation order", reservationOrder);
            if (ObjectIsValid(new ReservationOrderValidators(), reservationOrder))
            {
                ILogger.LogInformation("Start check availability on database");
                return IReservationRepository.CheckAvailability(reservationOrder);
            }

            ILogger.LogInformation("Entry object is not valid, the Check Room Availability result is going to be false");
            return false;
        }

        public bool CheckAvailabilityOnModifyingBooking(ReservationOrder reservationOrder)
        {
            if (ObjectIsValid(new ReservationOrderValidators(), reservationOrder))
            {
                return IReservationRepository.CheckAvailabilityOnModifyingBooking(reservationOrder);
            }
            return false;
        }
    }
}
