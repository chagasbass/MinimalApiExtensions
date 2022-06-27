using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace MinimalApi.Extensions
{
    public static class LogExtensions
    {
        public static IServiceCollection AddMinimalApiAspNetCoreHttpLogging(this IServiceCollection services)
        {
            services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
                                        HttpLoggingFields.ResponseStatusCode |
                                        HttpLoggingFields.ResponseBody |
                                        HttpLoggingFields.RequestBody;

                options.RequestBodyLogLimit = 4096;
                options.ResponseBodyLogLimit = 4096;
                options.MediaTypeOptions.AddText("application/json");
            });

            return services;
        }


        public static Logger ConfigureStructuralLogWithSerilog()
        {
            return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck-ui")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksDb")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksUI")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("healthchecks-data-ui")))
            .Destructure.ByTransforming<HttpRequest>(x => new
            {
                Method = x.Method,
                Url = x.Path,
                QueryString = x.QueryString
            })
            .WriteTo.Console()
            .CreateLogger();
        }
    }
}
