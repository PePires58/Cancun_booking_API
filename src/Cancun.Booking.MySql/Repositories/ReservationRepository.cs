using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.MySql.Context;
using System.Diagnostics.CodeAnalysis;

namespace Cancun.Booking.MySql.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ReservationRepository : RepositoryBase<ReservationOrder>, IReservationRepository
    {
        public ReservationRepository(CancunDbContext cancunDbContext) : base(cancunDbContext)
        {
        }

        public bool CheckAvailability(ReservationOrder reservationOrder)
        {
            return !Any(c =>
                c.RoomId == reservationOrder.RoomId &&
                BookingBetweenSomeDate(reservationOrder, c)
                );
        }

        public bool CheckAvailabilityOnModifyingBooking(ReservationOrder reservationOrder)
        {
            return Any(c =>
                c.RoomId == reservationOrder.RoomId &&
                BookingBetweenSomeDate(reservationOrder, c) &&
                c.CustomerEmail != reservationOrder.CustomerEmail
                );
        }

        #region Private methods
        private static bool BookingBetweenSomeDate(ReservationOrder reservationOrder, ReservationOrder c)
        {
            return (c.StartDate >= reservationOrder.StartDate && c.StartDate <= reservationOrder.EndDate) ||
                            (c.EndDate >= reservationOrder.StartDate && c.EndDate <= reservationOrder.EndDate) ||
                            (c.StartDate <= reservationOrder.StartDate && c.EndDate >= reservationOrder.EndDate) ||
                            (c.StartDate >= reservationOrder.StartDate && c.EndDate <= reservationOrder.EndDate);
        }
        #endregion
    }
}
