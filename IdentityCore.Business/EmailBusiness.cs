using IdentityCore.Business.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Cryptography;
using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;

namespace IdentityCore.Business
{
    public class EmailBusiness : BaseBusiness, IEmailBusiness
    {
        public EmailBusiness()
        {

        }

        public Task<bool> SendMailAsync(UserDTO user, TemplateEmailType templateType)
        {
            var message = new MimeMessage();

            var template = FetchTemplate(templateType, user);

            message.From.Add(new MailboxAddress(GlobalConfiguration.MailKit.FullName, GlobalConfiguration.MailKit.Email));
            message.To.Add(new MailboxAddress(user.FullName, user.Email));
            message.Subject = template.Title;            

            message.Body = new TextPart(GlobalConfiguration.MailKit.TextPart)
            {
                Text = template.Content
            };

            using (var client = new SmtpClient())
            {
                client.Connect(GlobalConfiguration.MailKit.Host, GlobalConfiguration.MailKit.Port, GlobalConfiguration.MailKit.UseSSL);
                client.Authenticate(GlobalConfiguration.MailKit.Email, GlobalConfiguration.MailKit.AppPassword);
                client.Send(message);
                client.Disconnect(true);
            }

            return Task.FromResult(true);
        }

        public string GenerateOTP(int sizeCode)
        {
            if (sizeCode <= 0)
            {
                return "";
            }

            byte[] randomBytes = new byte[sizeCode];

            ;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var otp = new char[sizeCode];
            for (int i = 0; i < sizeCode; i++)
            {
                otp[i] = (char)('0' + (randomBytes[i] % 10));
            }

            return new string(otp);
        }

        private TemplateDTO FetchTemplate(TemplateEmailType template, UserDTO user )
        {
            switch (template)
            {
                case TemplateEmailType.OTP:
                {
                    if(GlobalConfiguration.MailKit != null && GlobalConfiguration.MailKit.Templates != null)
                    {
                        var templateInfo = GlobalConfiguration.MailKit.Templates.Where(s => s.EmailType == template).FirstOrDefault();
                        if(templateInfo == null)
                        {
                            return null;
                        }

                        string content = File.ReadAllText(templateInfo.Located);
                        if (user != null)
                        {
                            content = content.Replace("{Name}", user.FullName)
                                            .Replace("{OTP}", user.OTPCode)
                                            .Replace("{Year}", DateTime.Now.Year.ToString());

                            return new TemplateDTO()
                            {
                                Content = content,
                                Title = templateInfo.Title
                            };
                        }  
                    }

                    return null;
                }
                case TemplateEmailType.ForgotPassword:
                {
                    string content = "";
                    return new TemplateDTO()
                    {
                        Content = content,
                        Title = "Do Some Thing"
                    };
                }
                default:
                {
                    string content = "";
                    return new TemplateDTO()
                    {
                        Content = content,
                        Title = "Do Some Thing"
                    };
                }                
            }
        }
    }
}
