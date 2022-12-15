using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Domain.Interfaces.Services
{
    public interface IRoomAvailabilityService
    {
        bool CheckRoomAvailability(ReservationOrder reservationOrder);
        bool CheckAvailabilityOnModifyingBooking(ReservationOrder reservationOrder);
    }
}
