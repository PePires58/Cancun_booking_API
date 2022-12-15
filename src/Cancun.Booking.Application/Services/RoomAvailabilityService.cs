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

        public bool CheckRoomAvailability(RoomAvailability roomAvailability)
        {
            if (ObjectIsValid(new RoomAvailabilityValidators(), roomAvailability))
            {
                return IReservationRepository.CheckAvailability(roomAvailability);
            }
            return false;
        }
    }
}
