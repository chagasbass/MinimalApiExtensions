using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace MinimalApi.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            var mensagemPadrao = "Não informado";

            var applicationName = configuration["BaseConfiguration:NomeAplicacao"];
            var applicationDescription = configuration["BaseConfiguration:Descricao"];
            var developerName = configuration["BaseConfiguration:NomeDesenvolvedor"];
            var companyName = configuration["BaseConfiguration:NomeEmpresa"];
            var companyUrl = configuration["BaseConfiguration:UrlEmpresa"];
            var hasAuthentication = bool.Parse(configuration["BaseCondfiguration:TemAutenticacao"]);

            if (string.IsNullOrEmpty(companyUrl))
                companyUrl = mensagemPadrao;

            if (string.IsNullOrEmpty(companyName))
                companyName = mensagemPadrao;

            #region Criar versões diferentes de rotas
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = applicationName,
                    Description = $"{applicationDescription} Developed by {developerName}",
                    License = new OpenApiLicense { Name = companyName, Url = new Uri(companyUrl) }
                });

                if (hasAuthentication)
                {
                    var securitySchema = new OpenApiSecurityScheme
                    {
                        Description = "Autorização efetuada via JWT token",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    };

                    c.AddSecurityDefinition("Bearer", securitySchema);

                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        { securitySchema, new[] { "Bearer" } }
                    };

                    c.AddSecurityRequirement(securityRequirement);
                }
            });

            #endregion

            return services;
        }
    }
}
