using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Extensions.Shared.Logs.Services;
using MinimalApi.Extensions.Shared.Notifications;
using MInimalApi.Extensions.Shared.Logs;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace MinimalApi.Extensions.Extensions
{
    public static class LogIntegrationsExtensions
    {
        public static Logger ConfigureLog()
        {
            var logger = new LoggerConfiguration()
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                 .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                 .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                 .MinimumLevel.Override("System", LogEventLevel.Error)
                 .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck")))
                 .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck-ui")))
                 .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksDb")))
                 .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksUI")))
                 .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("healthchecks-data-ui")))
                 .Enrich.FromLogContext()
                 .WriteTo.Console(new CompactJsonFormatter());

            return logger.CreateLogger();
        }

        public static IServiceCollection AddLogServiceDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ILogServices, LogServices>();
            services.AddSingleton<LogData>();

            return services;
        }

        public static IServiceCollection AddNotificationControl(this IServiceCollection services)
        {
            services.AddSingleton<INotificationServices, NotificationServices>();
            return services;
        }
    }
}
