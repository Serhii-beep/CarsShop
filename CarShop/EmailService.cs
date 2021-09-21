using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace CarShop
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string emailAddress = "";
            string password = "";
            using(StreamReader sr = new StreamReader("appsettings.json"))
            {
                string json = sr.ReadToEnd();
                dynamic list = JsonConvert.DeserializeObject(json);
                emailAddress = list.EmailData.Address;
                password = list.EmailData.Password;

            }
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("CarShop", emailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {                
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(emailAddress, password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}