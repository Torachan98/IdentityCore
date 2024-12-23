using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Business.Interfaces
{
    public interface ICronJobBusiness
    {
        Task CleanOTPAsync();
    }
}
