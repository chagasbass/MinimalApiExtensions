using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Extensions.Shared;
using MinimalApi.Shared;

namespace MinimalApi.Extensions
{
    public static class OptionsExtensions
    {
        public static IServiceCollection AddBaseConfigurationOptionsPattern(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BaseConfigurationOptions>(configuration.GetSection(BaseConfigurationOptions.BaseConfig));
            services.Configure<ProblemDetailConfigurationOptions>(configuration.GetSection(ProblemDetailConfigurationOptions.BaseConfig));

            return services;
        }
    }
}
