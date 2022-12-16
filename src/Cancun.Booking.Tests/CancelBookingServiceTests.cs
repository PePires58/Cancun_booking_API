using Cancun.Booking.Application.Services;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notification.Application.Services;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Tests
{
    [TestClass]
    public class CancelBookingServiceTests
    {
        #region Properties
        INotificatorService INotificatorService { get; set; }
        ICancelBookingService ICancelBookingService { get; set; }
        Mock<IReservationRepository> IReservationRepository { get; set; }

        static string CustomerEmail => "Customer@email.com";
        #endregion

        #region Constructor
        public CancelBookingServiceTests()
        {
            INotificatorService = new NotificatorService();
            IReservationRepository = new Mock<IReservationRepository>();
            ICancelBookingService = new CancelBookingService(INotificatorService, IReservationRepository.Object);
        }
        #endregion

        #region Methods
        [TestMethod]
        public void ShouldThrowAnArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                ICancelBookingService.CancelBooking(null);
            });
        }

        [TestMethod]
        public void ShouldHaveANotificationOfInvalidEmail() {
            ICancelBookingService.CancelBooking(new("email.com", 0));

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "Customer email is invalid"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfEmptyEmail()
        {
            ICancelBookingService.CancelBooking(new());

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "Customer email cannot be empty"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfNullEmail()
        {
            ICancelBookingService.CancelBooking(new() { CustomerEmail = null});

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "Customer email cannot be null"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfInvalidReservationId()
        {
            ICancelBookingService.CancelBooking(new("email.com", 0));

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "Reservation ID is invalid"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfNonExistReservation()
        {
            CancelReservationOrder cancelReservationOrder = new(CustomerEmail, 2);

            IReservationRepository.Setup(c => c.Any(c => c.Id == cancelReservationOrder.ReservationId))
                .Returns(false);
            ICancelBookingService.CancelBooking(cancelReservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "Reservation does not exists"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfReservationDoesNotBelongsToYou()
        {
            CancelReservationOrder cancelReservationOrder = new(CustomerEmail, 2);

            IReservationRepository.Setup(c => c.Any(c => c.Id == cancelReservationOrder.ReservationId))
                .Returns(true);
            IReservationRepository.Setup(c => c.Any(c => c.Id == cancelReservationOrder.ReservationId &&
                        c.CustomerEmail == cancelReservationOrder.CustomerEmail))
                .Returns(false);

            ICancelBookingService.CancelBooking(cancelReservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "You cannot cancel this reservation because it's not belongs to you"));
        }

        [TestMethod]
        public void ShouldCancelTheReservation()
        {
            CancelReservationOrder cancelReservationOrder = new(CustomerEmail, 2);
            ReservationOrder reservationOrder = new();

            IReservationRepository.Setup(c => c.Any(c => c.Id == cancelReservationOrder.ReservationId))
                .Returns(true);
            IReservationRepository.Setup(c => c.Any(c => c.Id == cancelReservationOrder.ReservationId &&
                        c.CustomerEmail == cancelReservationOrder.CustomerEmail))
                .Returns(true);
            IReservationRepository.Setup(c => c.GetById(cancelReservationOrder.ReservationId))
                .Returns(reservationOrder);
            IReservationRepository.Setup(c => c.Update(reservationOrder));
            IReservationRepository.Setup(c => c.Save());

            ICancelBookingService.CancelBooking(cancelReservationOrder);

            Assert.IsFalse(INotificatorService.HasNotification);
        }
        #endregion
    }
}
