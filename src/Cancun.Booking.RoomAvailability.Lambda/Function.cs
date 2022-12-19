using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Enums;
using Cancun.Booking.Domain.Interfaces.Services;
using Cancun.Booking.Shared;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Cancun.Booking.RoomAvailability.Lambda;

public class Function : DefaultLambdaWorkflow<ReservationOrder>
{
    #region Properties
    IRoomAvailabilityService IRoomAvailabilityService { get; set; }

    bool Available { get; set; }
    bool ModifyingBooking { get; set; }

    protected override APIGatewayProxyResponse Response => new()
    {
        StatusCode = 200,
        Body = JsonConvert.SerializeObject(new
        {
            Available
        })
    };

    #endregion

    #region Constructor

    public Function() : base(LambdaServices.CheckAvailability)
    {
        IRoomAvailabilityService = ServiceProvider.GetService<IRoomAvailabilityService>();
    }
    #endregion

    #region Methods

    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request,
                ILambdaContext context)
    {
        if (request.QueryStringParameters.TryGetValue("ModifyingBooking", out var value))
        {
            ModifyingBooking = value.Equals("TRUE", StringComparison.OrdinalIgnoreCase);
        }

        return DefaultLambdaHandler(request, context);
    }

    protected override void CallService(ReservationOrder reservationOrder)
    {
        Available = ModifyingBooking ? IRoomAvailabilityService.CheckAvailabilityOnModifyingBooking(reservationOrder)
            : IRoomAvailabilityService.CheckRoomAvailability(reservationOrder);
    }
    #endregion
}
