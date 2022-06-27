using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Extensions.CustomsResults;

namespace MinimalApi.Extensions.Extensions
{
    public static class CustomResultsExtensions
    {
        public static IServiceCollection AddApiCustomResults(this IServiceCollection services)
        {
            services.AddSingleton<IApiCustomResults, ApiCustomResults>();
            return services;
        }
    }
}
