using Cancun.Booking.Domain.Entities;
using FluentValidation;
using System.Net.Mail;

namespace Cancun.Booking.Application.Validators
{
    public class CancelReservationOrderValidators : AbstractValidator<CancelReservationOrder>
    {
        public CancelReservationOrderValidators()
        {
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    EnsureInstanceNotNull(c);
                });

            RuleFor(c => c.CustomerEmail)
                .NotEmpty().WithMessage("Customer email cannot be empty")
                .NotNull().WithMessage("Customer email cannot be null");

            When(c => !string.IsNullOrEmpty(c.CustomerEmail), () =>
            {
                RuleFor(c => c.CustomerEmail)
                .Custom((c, context) =>
                {
                    if (!MailAddress.TryCreate(c, out _))
                        context.AddFailure("Customer email is invalid");
                });
            });

            RuleFor(c => c.ReservationId)
                .GreaterThan(0).WithMessage("Reservation ID is invalid");
        }
    }
}
