using Cancun.Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Domain.Interfaces.Services
{
    public interface IRoomAvailabilityService
    {
        bool CheckRoomAvailability(RoomAvailability roomAvailability);
    }
}
