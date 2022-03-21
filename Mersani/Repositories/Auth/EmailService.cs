

using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mersani.Repositories.Auth
{
    public class AppSettings
    {
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }

    public class Email
    {
        public string USERNAME { get; set; }
        public string TO { get; set; }
        public string SUBJECT { get; set; }
        public string MESSAGE { get; set; }
    }

    public interface IEmailService
    {
        Task<bool> SendAsync(Email email, AppSettings settings);
    }

    public class EmailService : IEmailService
    {
        public async Task<bool> SendAsync(Email email, AppSettings settings)
        {
            var client = new SmtpClient("smtp-mail.outlook.com", 587);
            client.UseDefaultCredentials = true;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("ibrahimhamed2016@hotmail.com", "Roma01008928060");
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("@hotmail.com");
            mailMessage.To.Add(email.TO);
            mailMessage.Body = email.MESSAGE;
            mailMessage.Subject = email.SUBJECT;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            await client.SendMailAsync(mailMessage);
            client.Dispose();
            return true;
        }
    }
}
