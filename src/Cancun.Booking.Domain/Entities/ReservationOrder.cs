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
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StayDays => EndDate.Date.Subtract(StartDate.Date).Days;
        public string CustomerEmail { get; set; }
        public int RoomId { get { return 1; } } /*For porpuse tests, there is only one room*/
    }
}
