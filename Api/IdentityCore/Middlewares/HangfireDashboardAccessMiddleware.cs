namespace IdentityCore.Middlewares
{
    public class HangfireDashboardAccessMiddleware
    {
        private readonly RequestDelegate _next;

        public HangfireDashboardAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.IsRequestFor(HangfireConfig.DashboardUrl))
            {
                await _next.Invoke(context).ConfigureAwait(true);
                return;
            }

            // Set cookie if need
            string requestAccessKey = context.Request.Query[HangfireConfig.AccessKeyQueryParam]!;

            if (!string.IsNullOrWhiteSpace(requestAccessKey) && context.Request.Cookies[HangfireConfig.AccessKeyQueryParam] != requestAccessKey)
            {
                SetCookie(context, Const.CookieAccessKeyName, requestAccessKey);
            }

            // Check Permission
            bool isCanAccess = HangsfireHelper.IsCanAccessHangfireDashboard(context);

            if (!isCanAccess)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.Headers.Clear();
                await context.Response.WriteAsync(HangfireConfig.UnAuthorizeMessage).ConfigureAwait(true);
                return;
            }

            await _next.Invoke(context).ConfigureAwait(true);
        }

        private static void SetCookie(HttpContext context, string key, string value)
        {
            context.Response.Cookies.Append(key, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true // allow transmit via http and https
            });
        }
    }
}
