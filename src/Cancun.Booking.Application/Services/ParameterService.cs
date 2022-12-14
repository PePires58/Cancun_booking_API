using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Services;

namespace Cancun.Booking.Application.Services
{
    public class ParameterService : IParameterService
    {
        public Parameters GetParameters()
        {
            return new Parameters(maxStayDays: 3, maxDaysBookingAdvance: 30, minDaysBookingAdvance: 1);
        }
    }
}
