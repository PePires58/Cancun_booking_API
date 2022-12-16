using Cancun.Booking.Application.Services;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notification.Application.Services;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Tests
{
    [TestClass]
    public class PreBookingValidatorServiceTests
    {
        #region Properties
        IPreBookingValidatorService IPreBookingValidatorService { get; set; }
        INotificatorService INotificatorService { get; set; }
        IParameterService IParameterService { get; set; }

        static string CustomerEmail => "Customer@gmail.com";
        Parameters Parameters => IParameterService.GetParameters();
        #endregion

        #region Constructor
        public PreBookingValidatorServiceTests()
        {
            Environment.SetEnvironmentVariable("MINDAYSBOOKINGADVANCE", "1");
            Environment.SetEnvironmentVariable("MAXSTAYDAYS", "3");
            Environment.SetEnvironmentVariable("MAXDAYSBOOKINGADVANCE", "30");

            INotificatorService = new NotificatorService();
            IParameterService = new ParameterService();
            IPreBookingValidatorService = new PreBookingValidatorService(INotificatorService, IParameterService);
        }
        #endregion

        [TestMethod]
        public void ShouldThrowAnArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IPreBookingValidatorService.ValidateReservationOrder(null);
            });
        }

        [TestMethod]
        public void ShouldHaveANotificationOfEmptyEmail()
        {
            ReservationOrder reservationOrder = new();
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == "Customer email cannot be empty"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfNullEmail()
        {
            ReservationOrder reservationOrder = new()
            {
                CustomerEmail = null
            };
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == "Customer email cannot be null"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfInvalidEmail()
        {
            ReservationOrder reservationOrder = new(DateTime.Now, DateTime.Now, "invalidemail.com");
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == "Customer email is invalid"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfInvalidStartDate()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(-1), DateTime.Now, CustomerEmail);
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == "You cannot make a retroactive booking"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfStartDateGraterThanEndDate()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(1), DateTime.Now, CustomerEmail);
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == "End date cannot be lower than start date"));

        }

        [TestMethod]
        public void ShouldNotAllowBecauseTheStayIsLongerThanMaximumDaysAllowed()
        {
            int daysAdvance = Parameters.MinDaysBookingAdvance + 1;
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(daysAdvance), DateTime.Now.AddDays(Parameters.MaxStayDays + daysAdvance + 1), CustomerEmail);
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == $"You cannot stay longer than {Parameters.MaxStayDays} days"));
        }

        [TestMethod]
        public void ShouldNotAllowBecauseTheDaysOfAdvanceIsTooHigh()
        {
            int daysAdvance = Parameters.MaxDaysBookingAdvance + 1;
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(daysAdvance), DateTime.Now.AddDays(daysAdvance + 1), CustomerEmail);
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == $"You cannot booking with {daysAdvance} days in advance, the maximum days of advance is {Parameters.MaxDaysBookingAdvance}"));
        }

        [TestMethod]
        public void ShouldNotAllowBecauseTheDaysOfAdvanceIsTooLow()
        {
            ReservationOrder reservationOrder = new(DateTime.Now, DateTime.Now.AddDays(Parameters.MaxStayDays), CustomerEmail);
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == $"Your reservation need to start with at least {Parameters.MinDaysBookingAdvance} days of advance"));
        }

        [TestMethod]
        public void ShouldHaveNoNotifications()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance), DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance + Parameters.MaxStayDays), CustomerEmail);
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);

            Assert.IsFalse(INotificatorService.HasNotification);
        }
    }
}