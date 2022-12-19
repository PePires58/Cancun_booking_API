using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Enums;
using FluentValidation;
using System.Net.Mail;

namespace Cancun.Booking.Application.Validators
{
    internal class ReservationOrderValidators : AbstractValidator<ReservationOrder>
    {
        public ReservationOrderValidators()
        {
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    EnsureInstanceNotNull(c);

                    if (c.StartDate.Date > c.EndDate.Date)
                        context.AddFailure("End date cannot be lower than start date");
                });

            RuleFor(c => c.StartDate.Date)
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("You cannot make a retroactive booking");

            RuleFor(c => c.CustomerEmail)
                .NotEmpty().WithMessage("Customer email cannot be empty")
                .NotNull().WithMessage("Customer email cannot be null");

            When(c => !string.IsNullOrEmpty(c.CustomerEmail), () =>
            {
                RuleFor(c => c.CustomerEmail)
                    .Custom((email, context) =>
                    {
                        if (!MailAddress.TryCreate(email, out _))
                            context.AddFailure("Customer email is invalid");
                    });
            });

            RuleFor(c => c.Status)
                .Equal(ReservationOrderStatus.Reserved).WithMessage("Invalid status for reservation");
        }
    }
}
