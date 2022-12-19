using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Enums;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.SqlServer.Context;
using System.Diagnostics.CodeAnalysis;

namespace Cancun.Booking.SqlServer.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ReservationRepository : RepositoryBase<ReservationOrder>, IReservationRepository
    {
        public ReservationRepository(CancunDbContext cancunDbContext) : base(cancunDbContext)
        {
        }

        public bool CheckAvailability(ReservationOrder reservationOrder)
        {
            return !Any(c => c.RoomId == reservationOrder.RoomId &&
                            (c.StartDate >= reservationOrder.StartDate && c.StartDate <= reservationOrder.EndDate) ||
                            (c.EndDate >= reservationOrder.StartDate && c.EndDate <= reservationOrder.EndDate) ||
                            (c.StartDate <= reservationOrder.StartDate && c.EndDate >= reservationOrder.EndDate) ||
                            (c.StartDate >= reservationOrder.StartDate && c.EndDate <= reservationOrder.EndDate) &&
                            c.Status == ReservationOrderStatus.Reserved);
        }

        public bool CheckAvailabilityOnModifyingBooking(ReservationOrder reservationOrder)
        {
            return !Any(c => c.RoomId == reservationOrder.RoomId &&
                            (c.StartDate >= reservationOrder.StartDate && c.StartDate <= reservationOrder.EndDate) ||
                            (c.EndDate >= reservationOrder.StartDate && c.EndDate <= reservationOrder.EndDate) ||
                            (c.StartDate <= reservationOrder.StartDate && c.EndDate >= reservationOrder.EndDate) ||
                            (c.StartDate >= reservationOrder.StartDate && c.EndDate <= reservationOrder.EndDate) &&
                            c.Status == ReservationOrderStatus.Reserved &&
                c.CustomerEmail != reservationOrder.CustomerEmail);
        }
    }
}
