namespace Cancun.Booking.Domain.Entities
{
    public class Parameters
    {
        public Parameters(int maxStayDays, int maxDaysBookingAdvance, int minDaysBookingAdvance)
        {
            MaxStayDays = maxStayDays;
            MaxDaysBookingAdvance = maxDaysBookingAdvance;
            MinDaysBookingAdvance = minDaysBookingAdvance;
        }

        public int MaxStayDays { get; set; }
        public int MaxDaysBookingAdvance { get; set; }
        public int MinDaysBookingAdvance { get; set; }
    }
}
