using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Domain.Interfaces.Services
{
    public interface IPreBookingValidatorService
    {
        void ValidateReservationOrder(ReservationOrder reservationOrder);
    }
}
