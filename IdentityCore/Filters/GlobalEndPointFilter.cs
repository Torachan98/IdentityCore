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

                if (objectResult.StatusCode == StatusCodes.Status200OK)
                {
                    var globalResponse = new { isSucess = true, requestId = Guid.NewGuid().ToString(), data = objectResult.Value };
                    objectResult.Value = globalResponse;
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // This method is called after the action result has been executed
        }
    }
}
