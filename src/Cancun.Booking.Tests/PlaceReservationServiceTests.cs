using Cancun.Booking.Application.Services;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notification.Application.Services;
using Notification.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Tests
{
    [TestClass]
    public class PlaceReservationServiceTests
    {
        #region Properties
        INotificatorService INotificatorService { get; set; }
        IPreBookingValidatorService IPreBookingValidatorService { get; set; }
        IParameterService IParameterService { get; set; }
        IPlaceReservationService IPlaceReservationService { get; set; }
        Mock<IReservationRepository> IReservationRepository { get; set; }
        
        static string CustomerEmail => "Customer@email.com";
        Parameters Parameters { get; set; }
        #endregion

        #region Constructor
        public PlaceReservationServiceTests()
        {
            INotificatorService = new NotificatorService();
            IParameterService = new ParameterService();

            IPreBookingValidatorService = new PreBookingValidatorService(INotificatorService, IParameterService);

            IReservationRepository = new Mock<IReservationRepository>();

            IPlaceReservationService = new PlaceReservationService(INotificatorService, IPreBookingValidatorService, IReservationRepository.Object);

            Parameters = IParameterService.GetParameters();
        }
        #endregion

        #region Methods
        [TestMethod]
        public void ShouldThrowAnArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IPlaceReservationService.PlaceReservation(null);
            });
        }

        [TestMethod]
        public void ShouldHaveANotificationOfNotAvailableForThisDate()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance),
                DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance + Parameters.MaxStayDays),
                CustomerEmail);

            IReservationRepository.Setup(c => c.CheckAvailability(reservationOrder))
                .Returns(false);

            IPlaceReservationService.PlaceReservation(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(n => n.Message == $"The room {reservationOrder.RoomId} is not available for this date"));
        }

        [TestMethod]
        public void ShouldHaveNoNotifications()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance),
               DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance + Parameters.MaxStayDays),
               CustomerEmail);

            IReservationRepository.Setup(c => c.CheckAvailability(reservationOrder))
                .Returns(true);

            IPlaceReservationService.PlaceReservation(reservationOrder);

            Assert.IsFalse(INotificatorService.HasNotification);

        }
        #endregion
    }
}
