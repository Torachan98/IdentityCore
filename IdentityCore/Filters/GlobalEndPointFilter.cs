using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCore.Filters
{
    public class GlobalEndPointFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                switch (objectResult.StatusCode)
                {
                    case StatusCodes.Status500InternalServerError:
                    {
                        var globalResponse = new { isSucess = false, requestId = Guid.NewGuid().ToString(), message = objectResult.Value };
                        objectResult.Value = globalResponse;
                        break;
                    }
                    case StatusCodes.Status401Unauthorized:
                    {
                        var globalResponse = new { isSucess = false, requestId = Guid.NewGuid().ToString(), message = "You are not authorize" };
                        objectResult.Value = globalResponse;
                        break;
                    }
                    case int statusCode when statusCode == StatusCodes.Status400BadRequest || (statusCode > StatusCodes.Status401Unauthorized && statusCode <= StatusCodes.Status406NotAcceptable):
                    {
                        var globalResponse = new { isSucess = false, requestId = Guid.NewGuid().ToString(), data = objectResult.Value, statusCode = objectResult.StatusCode };
                        objectResult.StatusCode = 200;
                        objectResult.Value = globalResponse;
                        break;
                    }
                    default:
                    {
                        var globalResponse = new { isSucess = true, requestId = Guid.NewGuid().ToString(), data = objectResult.Value };
                        objectResult.Value = globalResponse;
                        break;
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // This method is called after the action result has been executed
        }
    }
}
