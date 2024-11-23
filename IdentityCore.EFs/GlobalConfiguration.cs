namespace IdentityCore.EFs
{
    public static class GlobalConfiguration
    {
        public static Jwt Jwt { get; set; }
        public static MailKits MailKit { get; set; }
        public static AccountLocked AccountLocked { get; set; }
        public static OTP OTP { get; set; }
    }

    public class Jwt
    {
        public string Key { get; set; } 
        public string Issuer { get; set; } 
        public string Audience { get; set; } 
        public int LifeTime { get; set; }
    }

    public class MailKits
    {
        public string TextPart { get; set; } 
        public string Host { get; set; } 
        public int Port { get; set; } 
        public bool UseSSL { get; set; } 
        public string Email { get; set; } 
        public string FullName { get; set; } 
        public string AppPassword { get; set; } 
        public List<Template> Templates { get; set; }
    }

    public class Template
    {
        public TemplateEmailType EmailType { get; set; }
        public string Title { get; set; } 
        public string Located { get; set; } 
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
