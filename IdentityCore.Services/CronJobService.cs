using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.Services.Interfaces;

namespace IdentityCore.Services
{
    public class CronJobService : ICronJobService
    {
        private readonly ICronJobBusiness _cronJobBusiness;

        private const string DoSomeThing = "DoSomeThing";

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
                    case DoSomeThing:
                    {
                        foreach (var cronExpression in cronJob.CronExpressions!)
                        {
                            RecurringJob.AddOrUpdate<ICronJobBusiness>($"{cronJob.Name}_{Guid.NewGuid()}",
                                x => _cronJobBusiness.CleanOTPAsync(), cronExpression, options);
                        }

                        break;
                    }
                }
            }
        }
    }
}
