namespace IdentityCore.EFs
{
    public static class GlobalConst
    {
        public static Jwt Jwt { get; set; } = new Jwt();
        public static MailKits MailKit { get; set; } = new MailKits();
        public static AccountLocked AccountLocked { get; set; } = new AccountLocked();
        public static OTP OTP { get; set; } = new OTP();
        public static Redis Redis { get; set; } = new Redis();
        public static CronJob CronJobs { get; set; } = new CronJob();
        public static Hangsfire Hangsfire { get; set; } = new Hangsfire();
    }

    public class Jwt
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int LifeTime { get; set; }
    }

    public class MailKits
    {
        public string TextPart { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } 
        public bool UseSSL { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string AppPassword { get; set; } = string.Empty;
        public List<Template> Templates { get; set; } = new List<Template>();
    }

    public class Template
    {
        public TemplateEmailType EmailType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Located { get; set; } = string.Empty;
    }

    public class AccountLocked
    {
        public int AttemptNumber { get; set; }
        public int LockedHour { get; set; }
    }  
    
    public class OTP
    {
        public int SizeCode { get; set; }
        public int LifeTimeMinute { get; set; }
    }

    public class Redis
    {
        public string Url { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
    }

    public class CronJob
    {
        public int DaysInWeek { get; set; }
        public List<Job> Jobs { get; set; } = new List<Job>();
    }

    public class Job
    {
        public bool IsEnable { get; set; }
        public string? Name { get; set; }
        public string[]? CronExpressions { get; set; }
    }

    public class Hangsfire
    {
        public string DashboardUrl { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string AccessKeyQueryParam { get; set; } = string.Empty;
        public string BackToSiteUrl { get; set; } = string.Empty;
    }
}
