using System.Net;

namespace AzureDBackup
{
    public class MailSettings
    {
        /// <summary>
        /// Outgoing server address
        /// </summary>
        public string SMTPServer { get; set; }
        /// <summary>
        /// Port, ex: 587
        /// </summary>
        public int SMTPServerPort { get; set; }
        /// <summary>
        /// SMTP credentials, username, password
        /// </summary>
        public NetworkCredential SMTPCredentials { get; set; }
        /// <summary>
        /// Sending and receiving e-mail address
        /// </summary>
        public string MailReceiver { get; set; }
    }
}