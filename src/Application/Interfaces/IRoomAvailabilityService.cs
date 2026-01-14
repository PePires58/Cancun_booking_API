using Application.Dto;

namespace Application.Interfaces
{
    public interface IRoomAvailabilityService
    {
        void CheckAvailability(ReservationOrderDto dto);
    }
}
