using Application.Dto;
using Application.Interfaces;
using Infra;

namespace Application.Services
{
    public class RoomAvailabilityService(CancunDbContext dbContext) : IRoomAvailabilityService
    {
        public void CheckAvailability(ReservationOrderDto dto)
        {
            if (dto.NewReservation)
            {
                bool alreadyReserved = dbContext.ReservationOrders.Any(c =>
                    c.StartDate <= dto.EndDate &&
                    dto.StartDate <= c.EndDate &&
                    c.RoomId == dto.RoomId &&
                    c.Status != Domain.Enuns.ReservationStatus.Canceled);

                if (alreadyReserved)
                    throw new Exception("Room is not available for the selected dates.");
            }
            else
            {
                bool alreadyReserved = dbContext.ReservationOrders.Any(c =>
                    c.StartDate <= dto.EndDate &&
                    dto.StartDate <= c.EndDate &&
                    c.RoomId == dto.RoomId &&
                    c.CustomerEmail != dto.CustomerEmail &&
                    c.Status != Domain.Enuns.ReservationStatus.Canceled);

                if (alreadyReserved)
                    throw new Exception("Room is not available for the selected dates.");
            }
        }
    }
}
