namespace Application.Exceptions
{
    public class ReservationNotFoundException : Exception
    {
        public ReservationNotFoundException() : base("Reservation not found.")
        {
        }

        public ReservationNotFoundException(string message) : base(message)
        {
        }

        public ReservationNotFoundException(int reservationId) 
            : base($"Reservation with ID {reservationId} was not found.")
        {
        }
    }
}
