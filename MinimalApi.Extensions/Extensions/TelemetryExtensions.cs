using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MInimalApi.Extensions.Extensions.Telemetry
{
    public static class TelemetryExtensions
    {
        public static IServiceCollection AddApplicationInsightsApiTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            var instrumentationKey = configuration["ApplicationInsights:InstrumentationKey"];

            var options = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
            {
                EnableAdaptiveSampling = true,
                InstrumentationKey = instrumentationKey,
                EnableHeartbeat = false
            };

            services.AddApplicationInsightsTelemetry(options);

            return services;
        }

        public static IServiceCollection AddApplicationInsightsWorkerTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            var instrumentationKey = configuration["ApplicationInsights:InstrumentationKey"];

            services.AddApplicationInsightsTelemetryWorkerService(new Microsoft.ApplicationInsights.WorkerService.ApplicationInsightsServiceOptions
            {
                InstrumentationKey = instrumentationKey
            });

            return services;
        }
    }
}
