using MInimalApi.Extensions.Shared.Logs;
using Serilog;
using Serilog.Context;
using System.Text;

namespace MinimalApi.Extensions.Shared.Logs.Services
{
    public class LogServices : ILogServices
    {
        public LogData LogData { get; set; }

        private readonly ILogger _logger = Log.ForContext<LogServices>();

        public LogServices()
        {
            LogData = new LogData();
        }

        public void WriteLog(string projectName)
        {
            if (LogData.ShouldProcessLog())
            {
                using (LogContext.PushProperty("Log da operação", projectName))
                {
                    _logger.Information("{@Timestamp}", LogData.Timestamp);
                    _logger.Information("{@Message}", LogData.LogMessage);
                    _logger.Information("{@TraceId}", LogData.TraceId);
                    _logger.Information("{@RequestInformation}", LogData.RequestInformation);
                    _logger.Information("{@ResponseInformation}", LogData.ResponseInformation);
                }

                LogData.ClearLogs();
            }
        }

        public void WriteLogWhenRaiseExceptions(string projectName)
        {
            using (LogContext.PushProperty("Log da operação", projectName))
            {

                if (LogData is not null)
                {
                    using (LogContext.PushProperty("Log da operação", projectName))
                    {
                        var logInformation = new StringBuilder();

                        logInformation.AppendLine($"[Exception]: {LogData.Exception.GetType().Name}");
                        logInformation.AppendLine($"[Exception Message]: {LogData.Exception.Message}");
                        logInformation.AppendLine($"[Exception StackTrace]: {LogData.Exception.StackTrace}");

                        if (LogData.Exception.InnerException is not null)
                        {
                            logInformation.AppendLine($"[InnerException]: {LogData.Exception?.InnerException?.Message}");
                        }

                        _logger.Error(logInformation.ToString());


                        LogData.ClearLogs();
                    }
                }
            }
        }

        public void CreateStructuredLog(LogData logData) => LogData = logData;

        public void WriteMessage(string mensagem) => _logger.Information($"{mensagem}");
    }
}
