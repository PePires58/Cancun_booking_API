using Cancun.Booking.Application.Validators;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Notification.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Services
{
    public class PlaceReservationService : BaseService, IPlaceReservationService
    {
        #region Properties
        IPreBookingValidatorService IPreBookingValidatorService { get; set; }
        IReservationRepository IReservationRepository { get; set; }
        #endregion

        #region Constructor
        public PlaceReservationService(INotificatorService INotificatorService,
            IPreBookingValidatorService IPreBookingValidatorService,
            IReservationRepository IReservationRepository) : base(INotificatorService)
        {
            this.IPreBookingValidatorService = IPreBookingValidatorService;
            this.IReservationRepository = IReservationRepository;
        }

        public void PlaceReservation(ReservationOrder reservationOrder)
        {
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);
            if (INotificatorService.HasNotification)
                return;

            if (IReservationRepository.CheckAvailability(reservationOrder))
            {
                IReservationRepository.Insert(reservationOrder);
                IReservationRepository.Save();
            }
            else
                HandleNotification($"The room {reservationOrder.RoomId} is not available for this date");
        }
        #endregion

    }
}
