using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace ereferee.Helpers
{
    public class NotificationHelper
    {
        public static async void SendEmail(string emailTo, string subject, string content)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_KEY");
            var sendKey = Environment.GetEnvironmentVariable("SENDGRID_EMAIL");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(sendKey, "eScout App");
            var to = new EmailAddress(emailTo, emailTo);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.ToString());
        }
    }
}