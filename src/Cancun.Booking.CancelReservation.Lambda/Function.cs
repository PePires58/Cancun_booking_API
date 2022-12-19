using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Notification.Domain.Interfaces;
using System.Net;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Shared;
using Cancun.Booking.Domain.Enums;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Cancun.Booking.CancelReservation.Lambda;

public class Function : DefaultLambdaWorkflow<CancelReservationOrder>
{

    #region Properties
    ICancelBookingService ICancelBookingService { get; set; }

    protected override APIGatewayProxyResponse Response => new()
    {
        StatusCode = (int)HttpStatusCode.OK
    };
    #endregion

    #region Constructor
    public Function() : base(LambdaServices.CancelReservation)
    {
        ICancelBookingService = ServiceProvider.GetService<ICancelBookingService>();
    }
    #endregion


    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        return DefaultLambdaHandler(request, context);
    }

    protected override void CallService(CancelReservationOrder cancelReservationOrder)
    {
        ICancelBookingService.CancelBooking(cancelReservationOrder);
    }
}
