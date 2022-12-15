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
    public class CancelBookingService : BaseService, ICancelBookingService
    {
        #region Properties
        IReservationRepository IReservationRepository { get; set; }
        #endregion
        public CancelBookingService(INotificatorService INotificatorService, IReservationRepository IReservationRepository)
            : base(INotificatorService)
        {
            this.IReservationRepository = IReservationRepository;
        }

        public void CancelBooking(CancelReservationOrder cancelReservationOrder)
        {
            if (ObjectIsValid(new CancelReservationOrderValidators(), cancelReservationOrder))
            {
                if (IReservationRepository.Any(c => c.Id == cancelReservationOrder.ReservationId))
                {
                    if (IReservationRepository.Any(c => c.Id == cancelReservationOrder.ReservationId &&
                        c.CustomerEmail == cancelReservationOrder.CustomerEmail))
                    {
                        IReservationRepository.Delete(cancelReservationOrder.ReservationId);
                    }
                    else
                        HandleNotification("You cannot cancel this reservation because it's not belongs to you");
                }
                else
                    HandleNotification("Reservation does not exists");
            }
        }
    }
}
