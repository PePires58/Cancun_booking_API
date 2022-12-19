using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Notification.Domain.Interfaces;
using System.Net;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Cancun.Booking.Domain.Entities;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Cancun.Booking.CancelReservation.Lambda;

public class Function
{

    #region Properties
    ICancelBookingService ICancelBookingService { get; set; }
    INotificatorService INotificatorService { get; set; }
    IServiceCollection IServiceCollection { get; set; }
    #endregion

    #region Constructor
    public Function()
    {
        IServiceCollection = ConfigureServices.Configure();

        ServiceProvider serviceProvider = IServiceCollection.BuildServiceProvider();
        INotificatorService = serviceProvider.GetService<INotificatorService>();
        ICancelBookingService  = serviceProvider.GetService<ICancelBookingService>();
    }
    #endregion


    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        if (request == null || request.Body == null)
            return new APIGatewayProxyResponse()
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

        try
        {
            CancelReservationOrder cancelReservationOrder = JsonConvert.DeserializeObject<CancelReservationOrder>(request.Body);

            if (cancelReservationOrder != null)
                ICancelBookingService.CancelBooking(
                    cancelReservationOrder
                    );
            else
                INotificatorService.HandleNotification("Entry object is invalid");

            if (INotificatorService.HasNotification)
                return new APIGatewayProxyResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = JsonConvert.SerializeObject(
                        INotificatorService.GetList()
                        )
                };

            return new APIGatewayProxyResponse()
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        catch (JsonException)
        {
            INotificatorService.HandleNotification("Entry object is invalid");
            return new APIGatewayProxyResponse()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Body = JsonConvert.SerializeObject(
                    INotificatorService.GetList()
                    )
            };
        }
        catch (Exception)
        {
            INotificatorService.HandleNotification("Something went wrong");
            return new APIGatewayProxyResponse()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Body = JsonConvert.SerializeObject(
                    INotificatorService.GetList()
                    )
            };
        }
    }
}
