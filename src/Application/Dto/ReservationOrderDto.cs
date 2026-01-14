using Domain.Entities;
using Domain.Enuns;

namespace Application.Dto
{
    public class ReservationOrderDto
    {
        public ReservationOrderDto()
        {
            RoomId = 1; // Default room ID for testing purposes
        }

        public bool NewReservation => Id == 0;
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public ReservationStatus Status { get; set; }

        public ReservationOrder MapToEntity()
        {
            return new ReservationOrder(Id, StartDate, EndDate, CustomerEmail, Status);
        }
    }
}
