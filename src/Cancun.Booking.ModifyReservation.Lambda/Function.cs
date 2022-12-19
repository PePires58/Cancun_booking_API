using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Enums;
using Cancun.Booking.Domain.Interfaces.Services;
using Cancun.Booking.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Cancun.Booking.ModifyReservation.Lambda;

public class Function : DefaultLambdaWorkflow<ReservationOrder>
{
    #region Properties
    IModifyReservationService? IModifyReservationService { get; set; }

    protected override APIGatewayProxyResponse Response => new()
    {
        StatusCode = (int)HttpStatusCode.OK
    };
    #endregion

    #region Constructor
    public Function() : base(LambdaServices.ModifyReservation)
    {
        IModifyReservationService = ServiceProvider.GetService<IModifyReservationService>();
    }
    #endregion

    #region Methods
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request,
               ILambdaContext context)
    {
        return DefaultLambdaHandler(request, context);
    }

    protected override void CallService(ReservationOrder entryObject)
    {
        IModifyReservationService?.ModifyReservation(entryObject);
    }
    #endregion

}
