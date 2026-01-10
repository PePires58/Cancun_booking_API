namespace Application.Exceptions
{
    public class InvalidReservationException : Exception
    {
        public InvalidReservationException() : base("Invalid reservation.")
        {
        }

        public InvalidReservationException(string message) : base(message)
        {
        }
    }
}
