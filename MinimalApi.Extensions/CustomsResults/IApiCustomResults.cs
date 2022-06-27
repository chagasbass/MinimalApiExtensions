using Microsoft.AspNetCore.Http;
using MinimalApi.Extensions.Entities;

namespace MinimalApi.Extensions.CustomsResults
{
    public interface IApiCustomResults
    {
        void GenerateLogResponse(CommandResult commandResult, int statusCode);
        IResult FormatApiResponse(CommandResult commandResult, string defaultEndpoint = null);
    }
}