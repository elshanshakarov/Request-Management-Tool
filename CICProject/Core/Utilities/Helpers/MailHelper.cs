using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Helpers
{
    public class MailHelper
    {
        public static void Send(string to, string from,string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Subject = subject;
            message.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential("fec73f5d13e62c", "5bc6f878081d6d");
            smtpClient.Port = 2525;
            smtpClient.Host = "sandbox.smtp.mailtrap.io";
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);
        }
    }
}
