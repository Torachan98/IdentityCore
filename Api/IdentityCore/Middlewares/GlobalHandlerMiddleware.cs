using Newtonsoft.Json;
using System.Net.Mime;
using System.Net;

namespace IdentityCore.Middlewares
{
    public class GlobalHandlerMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public GlobalHandlerMiddleware(ILogger<GlobalHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var response = new
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = ex.ToString()
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
