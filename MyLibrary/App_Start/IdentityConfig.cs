using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MyLibrary.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyLibrary
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage iMessage)

        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(iMessage.Destination));

            message.Subject = iMessage.Subject;
            message.IsBodyHtml = true;
            message.Body = iMessage.Body;
            
            message.From = new MailAddress("pisak.96@gmail.com", "MyLibrary Admin Oliwia");

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("pisak.96@gmail.com", Environment.GetEnvironmentVariable("tajna_zmienna_AP1"));
            var siemka = Environment.GetEnvironmentVariable("tajna_zmienna_AP1");
            //smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;


            //smtpClient.Send(message);
            
            using (smtpClient)
            {
                await smtpClient.SendMailAsync(message);
            }

            // SENDGRID TEZ DZIALA, ALE CZESTO MAIL LADUJE W SPAMIE/ JEST NIEDOSTARCZONY :(

            /*var client = new SendGridClient(Environment.GetEnvironmentVariable("tajna_zmienna_AP2"));
             // https://app.sendgrid.com

            var msg = new SendGridMessage()

            {

                From = new EmailAddress("oliwia.m.96@wp.pl", "MyLibrary Admin Oliwia M."),
               
                Subject = iMessage.Subject,

                PlainTextContent = iMessage.Body,

                HtmlContent = "<strong>" + iMessage.Body + "</strong>"

            };

            msg.AddTo(new EmailAddress(iMessage.Destination));

            var response = await client.SendEmailAsync(msg);
            */
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Dołącz tutaj usługę wiadomości SMS, aby wysłać wiadomość SMS.
            return Task.FromResult(0);
        }
    }

    // Skonfiguruj menedżera użytkowników aplikacji używanego w tej aplikacji. Interfejs UserManager jest zdefiniowany w produkcie ASP.NET Identity i jest używany przez aplikację.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Konfiguruj logikę weryfikacji nazw użytkowników
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Konfiguruj logikę weryfikacji haseł
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Konfiguruj ustawienia domyślne blokady użytkownika
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Zarejestruj dostawców uwierzytelniania dwuetapowego. W przypadku tej aplikacji kod weryfikujący użytkownika jest uzyskiwany przez telefon i pocztą e-mail
            // Możesz zapisać własnego dostawcę i dołączyć go tutaj.
            manager.RegisterTwoFactorProvider("Kod — telefon", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Twój kod zabezpieczający: {0}"
            });
            manager.RegisterTwoFactorProvider("Kod — e-mail", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Kod zabezpieczeń",
                BodyFormat = "Twój kod zabezpieczający: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Skonfiguruj menedżera logowania aplikacji używanego w tej aplikacji.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
