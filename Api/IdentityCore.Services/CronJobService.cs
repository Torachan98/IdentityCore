using Hangfire;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.Services.Interfaces;

namespace IdentityCore.Services
{
    public class CronJobService : ICronJobService
    {
        private readonly ICronJobBusiness _cronJobBusiness;

        private const string Cronjob_CleanupTokenInBlackList = "CleanupTokenInBlackList";
        private const string Cronjob_ResetOtpCode = "ResetOtpCode";

        public CronJobService(ICronJobBusiness cronJobBusiness) 
        { 
            _cronJobBusiness = cronJobBusiness;
        }

        public void RunCronJobs()
        {
            var cronJobPool = GlobalConst.CronJobs;
            var options = new RecurringJobOptions();
            options.TimeZone = TimeZoneInfo.Local;

            foreach (var cronJob in cronJobPool.Jobs!)
            {
                if (cronJob.IsEnable == false)
                    continue;

                switch (cronJob.Name)
                {
                    case Cronjob_CleanupTokenInBlackList:
                    {
                        foreach (var cronExpression in cronJob.CronExpressions!)
                        {
                            RecurringJob.AddOrUpdate<ICronJobBusiness>($"{cronJob.Name}_{Guid.NewGuid()}",
                                x => _cronJobBusiness.CleanBlackListAsync(), cronExpression, options);
                        }

                        break;
                    }

                    case Cronjob_ResetOtpCode:
                    {
                        foreach (var cronExpression in cronJob.CronExpressions!)
                        {
                            RecurringJob.AddOrUpdate<ICronJobBusiness>($"{cronJob.Name}_{Guid.NewGuid()}",
                                x => _cronJobBusiness.ResetOTPAsync(), cronExpression, options);
                        }

                        break;
                    }
                }
            }
        }
    }
}
