using Cancun.Booking.Application.Services;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Enums;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
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
    public class ModifyReservationServiceTests
    {
        #region Properties
        INotificatorService INotificatorService { get; set; }
        IParameterService IParameterService { get; set; }
        IPreBookingValidatorService IPreBookingValidatorService { get; set; }
        IModifyReservationService IModifyReservationService { get; set; }
        
        Parameters Parameters { get; set; }
        Mock<IReservationRepository> IReservationRepository { get; set; }

        static string CustomerEmail => "Customer@email.com";

        #endregion

        #region Constructor
        public ModifyReservationServiceTests()
        {
            EnvironmentVariablesForTesting.Configure();

            INotificatorService = new NotificatorService();
            IParameterService = new ParameterService();
            IPreBookingValidatorService = new PreBookingValidatorService(INotificatorService, IParameterService);

            Parameters = IParameterService.GetParameters();
            IReservationRepository = new Mock<IReservationRepository>();

            IModifyReservationService = new ModifyReservationService(INotificatorService, IPreBookingValidatorService, IReservationRepository.Object);
        }
        #endregion

        [TestMethod]
        public void ShouldThrowAnArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IModifyReservationService.ModifyReservation(null);
            });
        }

        [TestMethod]
        public void ShouldHaveANotificationOfInvalidEmail()
        {
            IModifyReservationService.ModifyReservation(new(DateTime.Now,DateTime.Now, "email.com"));

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "Customer email is invalid"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfEmptyEmail()
        {
            IModifyReservationService.ModifyReservation(new(DateTime.Now, DateTime.Now, String.Empty));

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "Customer email cannot be empty"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfNullEmail()
        {
            IModifyReservationService.ModifyReservation(new(DateTime.Now, DateTime.Now, String.Empty)
            {
                CustomerEmail = null
            });

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "Customer email cannot be null"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfNonExistReservation()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance),
                            DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance + Parameters.MaxStayDays), CustomerEmail)
            {
                Id = 2
            };

            IReservationRepository.Setup(c => c.Any(c => c.Id == reservationOrder.Id))
                .Returns(false);
            
            IModifyReservationService.ModifyReservation(reservationOrder);
            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "Reservation does not exists"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfReservationDoesNotBelongsToYou()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance),
                                        DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance + Parameters.MaxStayDays), CustomerEmail)
            {
                Id = 2
            };

            IReservationRepository.Setup(c => c.Any(c => c.Id == reservationOrder.Id))
                .Returns(true);
            IReservationRepository.Setup(c => c.Any(c => c.Id == reservationOrder.Id &&
                        c.CustomerEmail == reservationOrder.CustomerEmail))
                .Returns(false);

            IModifyReservationService.ModifyReservation(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "You cannot cancel this reservation because it's not belongs to you"));
        }

        [TestMethod]
        public void ShouldHaveANotificationOfAlreadyCancelled()
        {
            ReservationOrder reservationOrder = new(DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance),
                                        DateTime.Now.AddDays(Parameters.MinDaysBookingAdvance + Parameters.MaxStayDays), CustomerEmail)
            {
                Id = 2
            };

            IReservationRepository.Setup(c => c.Any(c => c.Id == reservationOrder.Id))
                .Returns(true);
            IReservationRepository.Setup(c => c.Any(c => c.Id == reservationOrder.Id &&
                        c.CustomerEmail == reservationOrder.CustomerEmail))
                .Returns(true);
            IReservationRepository.Setup(c => c.Any(c => c.Id == reservationOrder.Id &&
                c.Status == ReservationOrderStatus.Reserved))
                .Returns(false);

            IModifyReservationService.ModifyReservation(reservationOrder);

            Assert.IsTrue(INotificatorService.HasNotification);
            Assert.IsTrue(INotificatorService.Any(c => c.Message == "You cannot modify this reservation because it's already cancelled"));
        }


        [TestMethod]
        public void ShouldModifyTheReservation()
        {
            
        }
    }
}
