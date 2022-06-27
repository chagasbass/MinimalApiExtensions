using MinimalApi.Extensions.Shared.Helpers;

namespace MInimalApi.Extensions.Shared.Logs
{
    /// <summary>
    /// Representa o log estruturado.Pode ser alterado de acordo com a implementação
    /// </summary>
    public class LogData
    {
        public DateTime Timestamp { get; private set; }
        public object RequestInformation { get; private set; }
        public object ResponseInformation { get; private set; }
        public string? TraceId { get; private set; }
        public Exception Exception { get; private set; }
        public string? LogMessage { get; private set; }
        public bool HasLog { get; private set; } = true;
        public string? EndpointCall { get; private set; }
        public string? MethodEndpoint { get; private set; }
        public int StatusCode { get; private set; }

        public LogData()
        {
            Timestamp = DateTime.UtcNow.GetGmtDateTime();
        }

        public void AddDataLog(LogData logData)
        {
            Timestamp = logData.Timestamp;
            RequestInformation = logData.RequestInformation;
            TraceId = logData.TraceId;
            Exception = logData.Exception;
            LogMessage = logData.LogMessage;
            HasLog = logData.HasLog;
            EndpointCall = logData.EndpointCall;
            MethodEndpoint = logData.MethodEndpoint;
        }

        public LogData AddStatusCodeOperation(int statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        public LogData AddEndpointCall(string? endpointCall)
        {
            EndpointCall = endpointCall;
            return this;
        }

        public LogData AddMethodEndpoint(string? methodEndpoint)
        {
            MethodEndpoint = methodEndpoint;
            return this;
        }

        public bool ShouldProcessLog() => HasLog;

        public LogData HasLogInformation(bool existeLog)
        {
            HasLog = existeLog;
            return this;
        }

        public LogData AddLogTimestamp()
        {
            Timestamp = DateTime.UtcNow.GetGmtDateTime();

            return this;
        }

        public LogData AddLogMessage(string? message)
        {
            if (!string.IsNullOrEmpty(LogMessage))
                return this;

            LogMessage = message;
            return this;
        }

        public LogData AddRequestInformation(object requestInformation)
        {
            RequestInformation = requestInformation;
            return this;
        }

        public LogData AddResponseInformation(object responseInformation)
        {
            ResponseInformation = responseInformation;

            return this;
        }

        public LogData AddTraceIndendifier()
        {
            TraceId = Guid.NewGuid().ToString();
            return this;
        }

        public LogData AddException(Exception exception)
        {
            Exception = exception;
            return this;
        }

        public LogData ClearLogs()
        {
            Timestamp = DateTime.UtcNow.GetGmtDateTime();
            RequestInformation = string.Empty;
            TraceId = string.Empty;
            Exception = default;
            HasLog = false;
            LogMessage = string.Empty;
            MethodEndpoint = string.Empty;
            EndpointCall = string.Empty;

            return this;
        }
    }
}
