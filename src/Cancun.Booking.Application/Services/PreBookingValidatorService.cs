using Cancun.Booking.Application.Validators;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Notification.Domain.Interfaces;

namespace Cancun.Booking.Application.Services
{
    public class PreBookingValidatorService : BaseService, IPreBookingValidatorService
    {
        #region Properties
        Parameters Parameters { get; set; }
        ILogger<PreBookingValidatorService> ILogger { get; set; }
        #endregion

        public PreBookingValidatorService(INotificatorService INotificatorService, 
            IParameterService IParameterService,
            ILogger<PreBookingValidatorService> ILogger)
            : base(INotificatorService)
        {
            Parameters = IParameterService.GetParameters();
            this.ILogger = ILogger;
        }

        public void ValidateReservationOrder(ReservationOrder reservationOrder)
        {
            ILogger.LogInformation("Starting process of Validate Reservation Order");

            ILogger.LogInformation("Starting validation of entry object of reservation order", reservationOrder);
            if (ObjectIsValid(new ReservationOrderValidators(), reservationOrder))
            {
                ILogger.LogInformation("Starting business validation of booking");
                int daysAdvance = reservationOrder.StartDate.Date.Subtract(DateTime.Now.Date).Days;

                if (daysAdvance > Parameters.MaxDaysBookingAdvance)
                {
                    HandleNotification($"You cannot booking with {daysAdvance} days in advance, the maximum days of advance is {Parameters.MaxDaysBookingAdvance}");
                }
                else if (daysAdvance < Parameters.MinDaysBookingAdvance)
                {
                    HandleNotification($"Your reservation need to start with at least {Parameters.MinDaysBookingAdvance} days of advance");
                }
                else if (reservationOrder.StayDays > Parameters.MaxStayDays)
                {
                    HandleNotification($"You cannot stay longer than {Parameters.MaxStayDays} days");
                }
            }

            ILogger.LogInformation("Validation of entry object completed");
        }
    }
}