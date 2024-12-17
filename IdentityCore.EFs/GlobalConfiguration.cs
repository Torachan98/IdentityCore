namespace IdentityCore.EFs
{
    public static class GlobalConfiguration
    {
        public static Jwt Jwt { get; set; } = new Jwt();
        public static MailKits MailKit { get; set; } = new MailKits();
        public static AccountLocked AccountLocked { get; set; } = new AccountLocked();
        public static OTP OTP { get; set; } = new OTP();
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
}
