using MInimalApi.Extensions.Shared.Logs;

namespace MinimalApi.Extensions.Shared.Logs.Services
{
    public interface ILogServices
    {
        public LogData LogData { get; set; }
        void WriteLog(string projectName);
        void CreateStructuredLog(LogData logData);
        void WriteLogWhenRaiseExceptions(string projectName);
        void WriteMessage(string mensagem);
    }
}
