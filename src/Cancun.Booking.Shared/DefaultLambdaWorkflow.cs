using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Cancun.Booking.Domain.Enums;
using Cancun.Booking.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Notification.Domain.Interfaces;
using System.Net;

namespace Cancun.Booking.Shared
{
    public abstract class DefaultLambdaWorkflow<T>
        where T : class
    {
        #region Properties
        protected ServiceProvider ServiceProvider { get; set; }
        protected INotificatorService INotificatorService { get; set; }
        
        #endregion

        #region Constructor

        public DefaultLambdaWorkflow(LambdaServices lambdaServices)
        {
            ServiceProvider = ConfigureServices
                .Configure(lambdaServices)
                .BuildServiceProvider();

            INotificatorService = ServiceProvider.GetService<INotificatorService>();

        }
        #endregion

        #region Methods
        public APIGatewayProxyResponse DefaultLambdaHandler(APIGatewayProxyRequest request,
                ILambdaContext context)
        {
            if (request == null || request.Body == null)
                return new APIGatewayProxyResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

            try
            {
                T entryObject = JsonConvert.DeserializeObject<T>(request.Body);

                if (entryObject != null)
                    CallService(entryObject);
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

                return Response;
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
        #endregion

        protected abstract APIGatewayProxyResponse Response { get; }

        protected abstract void CallService(T entryObject);
    }
}