using Cancun.Booking.Domain.Enums;

namespace Cancun.Booking.Domain.Entities
{
    public class ReservationOrder
    {
        public ReservationOrder()
            : this(DateTime.Now, DateTime.Now, string.Empty) { }
        public ReservationOrder(DateTime startDate, DateTime endDate, string customerEmail)
        {
            StartDate = startDate;
            EndDate = endDate;
            CustomerEmail = customerEmail;
            Status = ReservationOrderStatus.Reserved;
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerEmail { get; set; }
        public ReservationOrderStatus Status { get; set; }

        public int StayDays => EndDate.Date.Subtract(StartDate.Date).Days;
        public int RoomId => 1; /*For porpuse tests, there is only one room*/
    }
}
