using System.Security.Cryptography;
using System.Text;

namespace IdentityCore.EFs.Helpers
{
    public static class EnscryptHelper
    {
        public static string ConvertSHA256(string value)
        {
            using (SHA256 hash = SHA256.Create())
            {
                return String.Concat(hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value.Trim()))
                  .Select(item => item.ToString("x2")));
            }
        }
    }
}
