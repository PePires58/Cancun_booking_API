using Cancun.Booking.Application.Services;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Moq;
using Notification.Application.Services;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Tests
{
    [TestClass]
    public class RoomAvailabilityServiceTests
    {
        #region Properties
        INotificatorService INotificatorService { get; set; }
        IRoomAvailabilityService IRoomAvailabilityService { get; set; }
        Mock<IReservationRepository> IReservationRepository { get; set; }
        static string CustomerEmail => "customer@email.com";
        #endregion

        #region Constructor
        public RoomAvailabilityServiceTests()
        {
            INotificatorService = new NotificatorService();
            IReservationRepository = new Mock<IReservationRepository>();
            IRoomAvailabilityService = new RoomAvailabilityService(INotificatorService, IReservationRepository.Object);
        }
        #endregion

        [TestMethod]
        public void ShouldThrowAnArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IRoomAvailabilityService.CheckRoomAvailability(null);
            });
        }

        [TestMethod]
        public void ShouldHaveANotificationOfInvalidStartDate()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(-1), DateTime.Now, CustomerEmail);
            IRoomAvailabilityService.CheckRoomAvailability(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == "You cannot make a retroactive booking"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfStartDateGraterThanEndDate()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(1), DateTime.Now, CustomerEmail);
            IRoomAvailabilityService.CheckRoomAvailability(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == "End date cannot be lower than start date"));
        }

        [TestMethod]
        public void ShouldReturnThatRoomIsNotAvailable() {
            ReservationOrder reservationOrder = new(DateTime.Now, DateTime.Now.AddDays(2), CustomerEmail);
            IReservationRepository.Setup(c => c.CheckAvailability(reservationOrder))
                .Returns(false);

            bool available = IRoomAvailabilityService.CheckRoomAvailability(reservationOrder);

            Assert.IsFalse(available);
            Assert.IsFalse(INotificatorService.HasNotification);
        }

        [TestMethod]
        public void ShouldReturnThatRoomIsAvailable() {
            ReservationOrder roomAvailability = new(DateTime.Now, DateTime.Now.AddDays(2), CustomerEmail);
            IReservationRepository.Setup(c => c.CheckAvailability(roomAvailability))
                .Returns(true);

            bool available = IRoomAvailabilityService.CheckRoomAvailability(roomAvailability);

            Assert.IsTrue(available);
            Assert.IsFalse(INotificatorService.HasNotification);
        }
    }
}
