using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Domain.Interfaces.Services
{
    public interface ICancelBookingService
    {
        void CancelBooking(CancelReservationOrder cancelReservationOrder);
    }
}
