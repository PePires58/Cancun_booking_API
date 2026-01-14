namespace Domain.Exceptions
{
    public class ReservationCannotBeUpdatedException : Exception
    {
        public ReservationCannotBeUpdatedException() 
            : base("The reservation cannot be updated because it is finished or canceled.")
        {
        }

        public ReservationCannotBeUpdatedException(string message) : base(message)
        {
        }
    }
}
