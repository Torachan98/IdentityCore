using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is FriendlyException exception)
            {
                //Handle friendly exception
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                var result = new { isSuccess = false, requestId = Guid.NewGuid().ToString(), exception.Code, exception.Message };
                context.Result = new JsonResult(result);
            }
            else
            {
                //Handle general exceptions
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = new { isSuccess = false, requestId = Guid.NewGuid().ToString(), code = StatusCodes.Status500InternalServerError, message = context.Exception.Message };
                context.Result = new JsonResult(result);
            }

            base.OnException(context);
        }
    }
}
