using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Notification.Domain.Interfaces;
using System.Net;
using System.Text.Json.Serialization;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Cancun.Booking.PlaceReservation.Lambda;

public class Function
{
    #region Properties
    IPlaceReservationService IPlaceReservationService { get; set; }
    INotificatorService INotificatorService { get; set; }
    IServiceCollection IServiceCollection { get; set; }
    #endregion

    #region Constructor
    public Function()
    {
        IServiceCollection = ConfigureServices.Configure();

        Environment.SetEnvironmentVariable("MINDAYSBOOKINGADVANCE", "1");
        Environment.SetEnvironmentVariable("MAXSTAYDAYS", "3");
        Environment.SetEnvironmentVariable("MAXDAYSBOOKINGADVANCE", "30");

        ServiceProvider serviceProvider = IServiceCollection.BuildServiceProvider();
        INotificatorService = serviceProvider.GetService<INotificatorService>();
        IPlaceReservationService = serviceProvider.GetService<IPlaceReservationService>();
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
            ReservationOrder reservationOrder = JsonConvert.DeserializeObject<ReservationOrder>(request.Body);

            if (reservationOrder != null)
                IPlaceReservationService.PlaceReservation(
                    reservationOrder
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
                StatusCode = (int)HttpStatusCode.Created
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
