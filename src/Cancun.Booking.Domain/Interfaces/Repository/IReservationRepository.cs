using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Domain.Interfaces.Repository
{
    public interface IReservationRepository
    {
        bool CheckAvailability(RoomAvailability roomAvailability);
    }
}
