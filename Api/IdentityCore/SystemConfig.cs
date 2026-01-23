using IdentityCore.EFs;

namespace IdentityCore
{
    public static class SystemConfig
    {
        public static void BuildSystemConfig(IConfiguration configuration)
        {
            GlobalConst.MailKit = configuration.GetSection<MailKits>(nameof(GlobalConst.MailKit)) ?? new MailKits();
            GlobalConst.Jwt = configuration.GetSection<Jwt>(nameof(GlobalConst.Jwt)) ?? new Jwt();
            GlobalConst.AccountLocked = configuration.GetSection<AccountLocked>(nameof(GlobalConst.AccountLocked)) ?? new AccountLocked();
            GlobalConst.OTP = configuration.GetSection<OTP>(nameof(GlobalConst.OTP)) ?? new OTP();
            GlobalConst.Redis = configuration.GetSection<Redis>(nameof(GlobalConst.Redis)) ?? new Redis();
            GlobalConst.CronJobs = configuration.GetSection<CronJob>(nameof(GlobalConst.CronJobs)) ?? new CronJob();
            GlobalConst.Hangsfire = configuration.GetSection<Hangsfire>(nameof(GlobalConst.Hangsfire)) ?? new Hangsfire();
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
