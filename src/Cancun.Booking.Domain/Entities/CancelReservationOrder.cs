namespace Cancun.Booking.Domain.Entities
{
    public class CancelReservationOrder
    {
        public CancelReservationOrder() : this(string.Empty, 0) { }

        public CancelReservationOrder(string customerEmail, int reservationId)
        {
            CustomerEmail = customerEmail;
            ReservationId = reservationId;
        }

        public string CustomerEmail { get; set; }
        public int ReservationId { get; set; }
    }
}
