using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Services;

namespace Cancun.Booking.Application.Services
{
    public class ParameterService : IParameterService
    {
        public Parameters GetParameters()
        {
            return new Parameters(maxStayDays: Convert.ToInt32(Environment.GetEnvironmentVariable("MAXSTAYDAYS")), 
                maxDaysBookingAdvance: Convert.ToInt32(Environment.GetEnvironmentVariable("MAXDAYSBOOKINGADVANCE")), 
                minDaysBookingAdvance: Convert.ToInt32(Environment.GetEnvironmentVariable("MINDAYSBOOKINGADVANCE")));
        }
    }
}
