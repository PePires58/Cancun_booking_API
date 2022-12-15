using Cancun.Booking.Application.Validators;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Application.Services
{
    public class RoomAvailabilityService : BaseService, IRoomAvailabilityService
    {
        #region Properties
        IReservationRepository IReservationRepository { get; set; }
        #endregion

        #region Constructor
        public RoomAvailabilityService(INotificatorService INotificatorService, IReservationRepository IReservationRepository)
            : base(INotificatorService)
        {
            this.IReservationRepository = IReservationRepository;

        }
        #endregion

        public bool CheckRoomAvailability(ReservationOrder reservationOrder)
        {
            if (ObjectIsValid(new ReservationOrderValidators(), reservationOrder))
            {
                return IReservationRepository.CheckAvailability(reservationOrder);
            }
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
