using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.Enums;
using Microsoft.Extensions.Caching.Distributed;

namespace IdentityCore.Business
{
    public class CronJobBusiness : ICronJobBusiness
    {
        private readonly IDistributedCache _distributedCache;
        public CronJobBusiness(IDistributedCache distributedCache) 
        {
            _distributedCache = distributedCache;
        }

        public async Task CleanOTPAsync()
        {
            await _distributedCache.RemoveAsync(KeyCache.BlackList);
        }
    }
}
