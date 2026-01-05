using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using IdentityCore.EFs.Requests;

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
                    var globalResponse = new ApiResponse<object> { IsSuccess = true, RequestId = Guid.NewGuid(), Data = objectResult.Value, Message = "Successfully" };
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
