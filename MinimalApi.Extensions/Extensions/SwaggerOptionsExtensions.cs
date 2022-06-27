using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MinimalApi.Extensions
{
    public class SwaggerOptionsExtensions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IConfiguration _configuration;

        public SwaggerOptionsExtensions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo(
                ApiVersionDescription description)
        {
            var applicationName = _configuration["BaseConfiguration:NomeAplicacao"];
            var applicationDescription = _configuration["BaseConfiguration:Descricao"];

            var info = new OpenApiInfo
            {
                Title = applicationName,
                Version = description.ApiVersion.ToString(),
                Description = applicationDescription
            };

            if (description.IsDeprecated)
            {
                info.Description += " Esta versão de API está depreciada.";
            }

            return info;
        }
    }
}
