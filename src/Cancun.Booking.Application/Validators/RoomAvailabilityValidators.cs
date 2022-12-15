using Cancun.Booking.Domain.Entities;
using FluentValidation;

namespace Cancun.Booking.Application.Validators
{
    public class RoomAvailabilityValidators : AbstractValidator<ReservationOrder>
    {
        public RoomAvailabilityValidators()
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

            RuleFor(c => c.RoomId)
                .GreaterThan(0).WithMessage("You must choose a room to make a booking");
        }
    }
}
