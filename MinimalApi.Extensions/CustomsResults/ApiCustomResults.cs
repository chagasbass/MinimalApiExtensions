using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MinimalApi.Extensions.Entities;
using MinimalApi.Extensions.Shared.Enums;
using MinimalApi.Extensions.Shared.Logs.Services;
using MinimalApi.Extensions.Shared.Notifications;
using MinimalApi.Shared;
using System.Net;

namespace MinimalApi.Extensions.CustomsResults
{
    public class ApiCustomResults : IApiCustomResults
    {
        private BaseConfigurationOptions _options;
        private readonly ILogServices _logServices;
        private readonly INotificationServices _notificationServices;

        public ApiCustomResults(IOptionsMonitor<BaseConfigurationOptions> options,
                                ILogServices logServices,
                                INotificationServices notificationServices)
        {
            _options = options.CurrentValue;
            _logServices = logServices;
            _notificationServices = notificationServices;
        }

        public IResult FormatApiResponse(CommandResult commandResult, string defaultEndpointRoute = null)
        {
            var statusCodeOperation = _notificationServices.StatusCode;

            if (_notificationServices.HasNotifications())
            {
                var notifications = _notificationServices.GetNotifications().ToList();

                commandResult.Data = notifications;

                _notificationServices.ClearNotifications();
            }

            switch (statusCodeOperation)
            {
                case var _ when statusCodeOperation == StatusCodeOperation.BadRequest:
                    GenerateLogResponse(commandResult, (int)HttpStatusCode.BadRequest);
                    return Results.BadRequest(commandResult);
                case var _ when statusCodeOperation == StatusCodeOperation.NotFound:
                    GenerateLogResponse(commandResult, (int)HttpStatusCode.NotFound);
                    return Results.NotFound(commandResult);
                case var _ when statusCodeOperation == StatusCodeOperation.Post:
                    GenerateLogResponse(commandResult, (int)HttpStatusCode.Created);
                    return Results.Created(defaultEndpointRoute, commandResult);
                case var _ when statusCodeOperation == StatusCodeOperation.Put:
                    GenerateLogResponse(commandResult, (int)HttpStatusCode.NoContent);
                    return Results.NoContent();
                case var _ when statusCodeOperation == StatusCodeOperation.Get:
                    GenerateLogResponse(commandResult, (int)HttpStatusCode.OK);
                    return Results.Ok(commandResult);
                default:
                    GenerateLogResponse(commandResult, (int)HttpStatusCode.OK);
                    return Results.Ok(commandResult);
            }
        }

        public void GenerateLogResponse(CommandResult commandResult, int statusCode)
        {
            _logServices.LogData.AddStatusCodeOperation(statusCode);
            _logServices.LogData.AddResponseInformation(commandResult);
            _logServices.WriteLog(_options.NomeAplicacao);
        }
    }
}
