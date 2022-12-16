using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Domain.Interfaces.Services
{
    public interface IModifyReservationService
    {
        void ModifyReservation(ReservationOrder reservationOrder);
    }
}
