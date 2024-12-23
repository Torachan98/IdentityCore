using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityCore.Business.Interfaces;

namespace IdentityCore.Business
{
    public class CronJobBusiness : ICronJobBusiness
    {
        public Task CleanOTPAsync()
        {
            throw new NotImplementedException();
        }
    }
}
