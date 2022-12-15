using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Domain.Interfaces.Services
{
    public interface IPlaceReservationService
    {
        void PlaceReservation(ReservationOrder reservationOrder);
    }
}
