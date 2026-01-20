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

        private int _roomId;
        public int RoomId
        {
            get
            {
                if (_roomId <= 0)
                    _roomId = 1;
                return _roomId;
            }
            set
            {
                if (value <= 0)
                    _roomId = 1;
                else
                    _roomId = value;
            }
        }
        public ReservationStatus Status { get; set; }

        public ReservationOrder MapToEntity()
        {
            return new ReservationOrder(Id, StartDate, EndDate, CustomerEmail, Status);
        }
    }
}
