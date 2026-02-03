using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Services.Interfaces
{
    public interface IFCMService
    {
        Task SendAsync(string fcmToken, string title, string body, Dictionary<string, string>? data = null);
    }
}
