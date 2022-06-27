using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MinimalApi.Extensions.Entities;
using MinimalApi.Extensions.Shared.Logs.Services;
using MinimalApi.Shared;
using System.Text.Json;

namespace MinimalApi.Extensions.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly BaseConfigurationOptions _options;
        private readonly ILogServices _logServices;

        public GlobalExceptionHandlerMiddleware(IOptions<BaseConfigurationOptions> options, ILogServices logServices)
        {
            _options = options.Value;
            _logServices = logServices;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Responsavel por tratar as exceções geradas na aplicação
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            const int statusCode = StatusCodes.Status500InternalServerError;
            const string dataType = @"application/problem+json";

            _logServices.LogData.AddException(exception);
            _logServices.LogData.AddStatusCodeOperation(statusCode);
            _logServices.WriteLog(_options.NomeAplicacao);
            _logServices.WriteLogWhenRaiseExceptions(_options.NomeAplicacao);

            var problemDetails = new ProblemDetails
            {
                Title = "Um erro ocorreu ao processar o request.",
                Status = statusCode,
                Type = exception.GetBaseException().GetType().Name,
                Detail = $"Erro fatal na aplicação,entre em contato com um Desenvolvedor responsável. Causa: {exception.Message}",
                Instance = context.Request.HttpContext.Request.Path
            };

            var commandResult = new CommandResult(problemDetails);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = dataType;

            await context.Response.WriteAsync(JsonSerializer.Serialize(commandResult));
        }
    }
}
