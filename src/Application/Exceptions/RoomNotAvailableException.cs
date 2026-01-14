namespace Application.Exceptions
{
    public class RoomNotAvailableException : Exception
    {
        public RoomNotAvailableException() : base("Room is not available for the selected dates.")
        {
        }

        public RoomNotAvailableException(string message) : base(message)
        {
        }
    }
}
