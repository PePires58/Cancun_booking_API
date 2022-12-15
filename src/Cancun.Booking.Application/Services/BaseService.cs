using FluentValidation;
using FluentValidation.Results;
using Notification.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Cancun.Booking.Application.Services
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseService
    {
        #region Properties
        protected INotificatorService INotificatorService { get; set; }
        #endregion

        public BaseService(INotificatorService INotificatorService)
        {
            this.INotificatorService = INotificatorService;
        }

        protected void HandleNotification(ValidationResult validationResult)
        {
            if (validationResult != null && validationResult.Errors != null)
                foreach(ValidationFailure error in validationResult.Errors)
                    HandleNotification(error.ErrorMessage);
        }

        protected void HandleNotification(string notification)
        {
            INotificatorService.HandleNotification(notification);
        }

        protected bool ObjectIsValid<TV, TE>(TV pValidator, TE pEntity)
            where TV : AbstractValidator<TE>
            where TE : class, new()
        {
            ValidationResult validationResult = pValidator.Validate(pEntity);
            HandleNotification(validationResult);
            return validationResult.IsValid;
        }
    }
}
