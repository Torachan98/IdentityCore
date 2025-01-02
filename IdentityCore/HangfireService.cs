using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.Server;
using Hangfire.States;
using Hangfire;
using System.Diagnostics.CodeAnalysis;
using RouteCollection = Hangfire.Dashboard.RouteCollection;
using Hangfire.MemoryStorage;

namespace IdentityCore
{
    public static class Const
    {
        public const string CookieAccessKeyName = "Hangfire_AccessKey";
        public const string DefaultConfigSection = "Hangfire";
    }

    public static class HangfireService
    {
        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration, string databaseConnectionString = null, string configSection = Const.DefaultConfigSection)
        {
            BuildHangfireConfig(configuration);
            // Check if already have hangfire service before, let remove to override purpose.
            services.Removes(typeof(JobStorage),
                typeof(IGlobalConfiguration),
                typeof(JobActivator),
                typeof(RouteCollection),
                typeof(IJobFilterProvider),
                typeof(IBackgroundJobFactory),
                typeof(IBackgroundJobStateChanger),
                typeof(IBackgroundJobPerformer),
                typeof(IBackgroundJobClient),
                typeof(IRecurringJobManager),
                typeof(IBackgroundProcess),
                typeof(Action<IGlobalConfiguration>));

            // Add Hangfire Service
            if (string.IsNullOrWhiteSpace(databaseConnectionString))
            {
                services.AddHangfire(config =>
                {
                    config.UseMemoryStorage();
                });
            }
            else
            {
                services.AddHangfire(config =>
                {
                    config.UseSqlServerStorage(databaseConnectionString);
                });
            }
            return services;
        }

        private static IServiceCollection Removes(this IServiceCollection services, params Type[] removeTypes)
        {
            foreach (var removeType in removeTypes)
            {
                var removeTypeFound = services.FirstOrDefault(x => x.ServiceType == removeType);

                if (removeTypeFound != null)
                {
                    services.Remove(removeTypeFound);
                }
            }
            return services;
        }

        public static void BuildHangfireConfig(this IConfiguration configuration, string configSection = Const.DefaultConfigSection)
        {
            HangfireConfig.DashboardUrl = configuration.GetValue($"{configSection}:{nameof(HangfireConfig.DashboardUrl)}", HangfireConfig.DashboardUrl)!;
            if (!HangfireConfig.DashboardUrl.StartsWith("/"))
            {
                throw new ArgumentException($"{nameof(HangfireConfig.DashboardUrl)} must start by /", nameof(HangfireConfig.DashboardUrl));
            }

            HangfireConfig.AccessKey = configuration.GetValue($"{configSection}:{nameof(HangfireConfig.AccessKey)}", HangfireConfig.AccessKey)!;
            HangfireConfig.AccessKeyQueryParam = configuration.GetValue($"{configSection}:{nameof(HangfireConfig.AccessKeyQueryParam)}", HangfireConfig.AccessKeyQueryParam)!;
            HangfireConfig.UnAuthorizeMessage = configuration.GetValue($"{configSection}:{nameof(HangfireConfig.UnAuthorizeMessage)}", HangfireConfig.UnAuthorizeMessage)!;
            HangfireConfig.BackToSiteUrl = configuration.GetValue($"{configSection}:{nameof(HangfireConfig.BackToSiteUrl)}", HangfireConfig.BackToSiteUrl)!;
            HangfireConfig.StatsPollingInterval = configuration.GetValue($"{configSection}:{nameof(HangfireConfig.StatsPollingInterval)}", HangfireConfig.StatsPollingInterval);
            string Name = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        }
    }

    public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return HangsfireHelper.IsCanAccessHangfireDashboard(httpContext);
        }

        
    }

    public static class HangsfireHelper
    {
        public static bool IsCanAccessHangfireDashboard(HttpContext httpContext)
        {
            if (string.IsNullOrWhiteSpace(HangfireConfig.AccessKeyQueryParam))
            {
                return true;
            }

            string requestKey = httpContext.Request.Query[HangfireConfig.AccessKeyQueryParam]!;
            requestKey = string.IsNullOrWhiteSpace(requestKey) ? httpContext.Request.Cookies[Const.CookieAccessKeyName]! : requestKey;
            var isCanAccess = string.IsNullOrWhiteSpace(HangfireConfig.AccessKey) || HangfireConfig.AccessKey == requestKey;
            return isCanAccess;
        }
    }

    public static class HangfireConfig
    {
        /// <summary>
        ///     Hangfire Dashboard Url. Ex: /developers/job, if this is <c> empty </c> then disable dashboard
        /// </summary>
        /// <remarks> Start with <c> "/" </c> but end with <c> empty </c>, default is "/developers/job" </remarks>
        public static string DashboardUrl { get; set; } = "/developers/job";

        /// <summary>
        ///     Access Key read from URI 
        /// </summary>
        /// <remarks> Empty is allow <c> Anonymous </c> </remarks>
        public static string AccessKey { get; set; } = "261073FA-D236-4F04-9D53-3E4CAC686FA1";

        /// <summary>
        ///     Query parameter via http request 
        /// </summary>
        /// <remarks> Empty is allow <c> Anonymous </c> </remarks>
        public static string AccessKeyQueryParam { get; set; } = "key";

        /// <summary>
        ///     Un-authorize message when user access api document with not correct key. Default is
        ///     "You don't have permission to Job Dashboard, please contact your administrator."
        /// </summary>
        public static string UnAuthorizeMessage { get; set; } = "You don't have permission to Job Dashboard, please contact your administrator.";

        /// <summary>
        ///     The path for the Back To Site link. Set to <see langword="null" /> in order to hide
        ///     the Back To Site link. Default is "/"
        /// </summary>
        public static string BackToSiteUrl { get; set; } = "/";

        /// <summary>
        ///     The interval the /stats endpoint should be polled with (milliseconds). Default is 2000
        /// </summary>
        public static int StatsPollingInterval { get; set; } = 2000;
    }
}

