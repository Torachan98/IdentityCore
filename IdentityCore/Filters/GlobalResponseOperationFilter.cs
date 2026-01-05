using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IdentityCore.Filters
{
    public class GlobalResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var response in operation.Responses.Values)
            {
                if (response.Content.TryGetValue("application/json", out var mediaType))
                {
                    mediaType.Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties =
                    {
                        ["isSuccess"] = new OpenApiSchema { Type = "boolean" },
                        ["requestId"] = new OpenApiSchema { Type = "string" },
                        ["message"] = new OpenApiSchema { Type = "string" },
                        ["data"] = mediaType.Schema
                    }
                    };
                }
            }
        }
    }
}
