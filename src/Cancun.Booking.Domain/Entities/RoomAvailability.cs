namespace Cancun.Booking.Domain.Entities
{
    public class RoomAvailability
    {
        public RoomAvailability() : this(DateTime.Now, DateTime.Now, 0) { }

        public RoomAvailability(DateTime startDate, DateTime endDate, int roomId)
        {
            StartDate = startDate;
            EndDate = endDate;
            RoomId = roomId;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RoomId { get; set; }
    }
}
