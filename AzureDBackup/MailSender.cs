using System.Net;
using System.Net.Mail;
using System.Text;

namespace AzureDBackup
{
    public class MailSender
    {
        public string Body { get; set; }
        public string Subject { get; set; }

       /// <summary>
       /// Send e-mail
       /// </summary>
       /// <param name="toEmail"></param>
       /// <param name="sender"></param>
       /// <param name="smtpServer"></param>
       /// <param name="port"></param>
       /// <param name="credentials"></param>
       /// <param name="enableSSL"></param>
        public void Send(string toEmail, string sender, string smtpServer, int port, NetworkCredential credentials,
                         bool enableSSL = true)
        {
            var message = new MailMessage();

            message.Body = Body;
            message.Subject = Subject;
            message.From = new MailAddress(sender);
            message.To.Add(new MailAddress(toEmail));
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;
            var client = new SmtpClient();
            client.Host = smtpServer;
            client.Credentials = credentials;
            client.Port = port;
            client.EnableSsl = enableSSL;
            client.Send(message);
        }
        /// <summary>
        /// Static e-mail method
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="message"></param>
        /// <param name="subject"></param>
        /// <param name="smtpServer"></param>
        /// <param name="port"></param>
        /// <param name="credential"></param>
        public static void SendMail(string to, string from, string message, string subject, string smtpServer, int port,
                                    NetworkCredential credential)
        {
            var ms = new MailSender();
            ms.Body = message;
            ms.Subject = subject;
            ms.Send(to, from, smtpServer, port, credential);
        }
    }
}