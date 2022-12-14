using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Enums;
using Cancun.Booking.Domain.Interfaces.Services;
using Cancun.Booking.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Cancun.Booking.PlaceReservation.Lambda;

public class Function : DefaultLambdaWorkflow<ReservationOrder>
{
    #region Properties
    IPlaceReservationService IPlaceReservationService { get; set; }

    protected override APIGatewayProxyResponse Response => new()
    {
        StatusCode = (int)HttpStatusCode.Created
    };
    #endregion

    #region Constructor
    public Function() : base(LambdaServices.PlaceReservation)
    {
        IPlaceReservationService = ServiceProvider.GetService<IPlaceReservationService>();
    }
    #endregion

    #region Methods
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        return DefaultLambdaHandler(request, context);
    }

    protected override void CallService(ReservationOrder reservationOrder)
    {
        IPlaceReservationService.PlaceReservation(reservationOrder);
    }
    #endregion

}
