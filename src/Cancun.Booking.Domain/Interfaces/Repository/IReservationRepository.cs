using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Domain.Interfaces.Repository
{
    public interface IReservationRepository : IRepositoryBase<ReservationOrder>
    {
        bool CheckAvailability(ReservationOrder reservationOrder);
        bool CheckAvailabilityOnModifyingBooking(ReservationOrder reservationOrder);
    }
}
