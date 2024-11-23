using IdentityCore.EFs;

namespace IdentityCore
{
    public static class SystemConfig
    {
        public static void BuildSystemConfig(IConfiguration configuration)
        {
            GlobalConfiguration.MailKit = configuration.GetSection<MailKits>(nameof(GlobalConfiguration.MailKit)) ?? new MailKits();
            GlobalConfiguration.Jwt = configuration.GetSection<Jwt>(nameof(GlobalConfiguration.Jwt)) ?? new Jwt();
            GlobalConfiguration.AccountLocked = configuration.GetSection<AccountLocked>(nameof(GlobalConfiguration.AccountLocked)) ?? new AccountLocked();
            GlobalConfiguration.OTP = configuration.GetSection<OTP>(nameof(GlobalConfiguration.OTP)) ?? new OTP();
        }

        public static T GetSection<T>(this IConfiguration configuration, string key = null) where T : new()
        {
            if (string.IsNullOrWhiteSpace(key))
                key = typeof(T).Name;

            var value = new T();
            configuration.GetSection(key).Bind(value);
            return value;
        }
    }
}
