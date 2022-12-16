using Cancun.Booking.Domain.Entities;
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
                .NotNull().WithMessage("Customer email cannot be null")
                .Custom((email, context) =>
                {
                    if (!MailAddress.TryCreate(email, out _))
                        context.AddFailure("Customer email is invalid");
                });
        }
    }
}
