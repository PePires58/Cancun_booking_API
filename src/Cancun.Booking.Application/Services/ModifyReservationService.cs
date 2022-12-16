using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.Domain.Interfaces.Services;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Application.Services
{
    public class ModifyReservationService : BaseService, IModifyReservationService
    {
        #region Properties

        private IPreBookingValidatorService IPreBookingValidatorService;
        private IReservationRepository IReservationRepository;
        #endregion

        #region Constructor

        public ModifyReservationService(INotificatorService INotificatorService, 
            IPreBookingValidatorService IPreBookingValidatorService, 
            IReservationRepository IReservationRepository) : base(INotificatorService)
        {
            this.IPreBookingValidatorService = IPreBookingValidatorService;
            this.IReservationRepository = IReservationRepository;
        }
        #endregion


        public void ModifyReservation(ReservationOrder reservationOrder)
        {
            IPreBookingValidatorService.ValidateReservationOrder(reservationOrder);
            if (IReservationRepository.Any(c => c.Id == reservationOrder.Id))
            {
                if (IReservationRepository.Any(c => c.Id == reservationOrder.Id &&
                    c.CustomerEmail == reservationOrder.CustomerEmail))
                {
                    if (IReservationRepository.CheckAvailabilityOnModifyingBooking(reservationOrder))
                    {
                        ReservationOrder reservationOrderDb = IReservationRepository.GetById(reservationOrder.Id);

                        reservationOrderDb.StartDate = reservationOrder.StartDate;
                        reservationOrderDb.EndDate = reservationOrder.EndDate;

                        IReservationRepository.Update(reservationOrderDb);
                        IReservationRepository.Save();
                    }
                    else
                        HandleNotification($"The room {reservationOrder.RoomId} is not available for this date");
                }
                else
                    HandleNotification("You cannot cancel this reservation because it's not belongs to you");
            }
            else
                HandleNotification("Reservation does not exists");
        }
    }
}
