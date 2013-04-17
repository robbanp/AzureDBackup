using System;
using System.IO;
using Microsoft.SqlServer.Dac;
using Quartz;

namespace AzureDBackup
{
    /// <summary>
    /// Backup job class
    /// </summary>
    [DisallowConcurrentExecution] // one concurrent job at the time
    internal class DbBackup : IJob
    {
        private BackupContext backupContext { get; set; }

        /// <summary>
        /// Is fired on job execution
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            backupContext = (BackupContext) context.JobDetail.JobDataMap.Get("bctx");
            Backup();
        }

        /// <summary>
        /// Db backup procedure
        /// </summary>
        public void Backup()
        {
            string folder = Environment.GetEnvironmentVariable("TEMP"); // Windows temp folder
            string fileName = DateTime.UtcNow.Ticks + backupContext.DatabaseName.ToLower() + ".bacpac";
            try
            {
                var svc = new DacServices(backupContext.DatabaseConnection);
                svc.Message += receiveDacServiceMessageEvent;
                svc.ProgressChanged += receiveDacServiceProgessEvent;
                Console.WriteLine("\n\rPerforming Export of {0} to {1} at {2}", backupContext.DatabaseName, fileName,
                                  DateTime.Now.ToLongTimeString());

                svc.ExportBacpac(folder + "/" + fileName, backupContext.DatabaseName);
                //Push to storage
                var storage = new StorageHandler(backupContext.StorageAccount);
                storage.SetBlob(backupContext.StorageContainer, folder + "/" + fileName);
                var f = new FileInfo(folder + "/" + fileName);

                if (backupContext.MailSetting.SMTPServer != null && backupContext.MailSetting.MailReceiver != null)
                {
                    //Change here for custom mail message
                    MailSender.SendMail(backupContext.MailSetting.MailReceiver,
                                        backupContext.MailSetting.MailReceiver,
                                        "Backup location: " + backupContext.StorageContainer + "/" + fileName +
                                        "\n\n File Size: " + f.Length.ToString() + " bytes, Time (UTC): " +
                                        DateTime.UtcNow,
                                        "Backup generated for " + backupContext.DatabaseName,
                                        backupContext.MailSetting.SMTPServer,
                                        backupContext.MailSetting.SMTPServerPort,
                                        backupContext.MailSetting.SMTPCredentials);
                }
                File.Delete(folder + "/" + fileName);
            }
            catch (Exception ex) // on error
            {
                if (backupContext.MailSetting.SMTPServer != null && backupContext.MailSetting.MailReceiver != null)
                {
                    //Change here for custom mail message
                    MailSender.SendMail(backupContext.MailSetting.MailReceiver,
                                        backupContext.MailSetting.MailReceiver,
                                        "Backup location: " + backupContext.StorageContainer + "/" + fileName +
                                        "\n\n Time (UTC): " + DateTime.UtcNow + "\n\n Exception: " + ex,
                                        "Error Backup generated for " + backupContext.DatabaseName,
                                        backupContext.MailSetting.SMTPServer,
                                        backupContext.MailSetting.SMTPServerPort,
                                        backupContext.MailSetting.SMTPCredentials);
                }
            }
        }

        private static void receiveDacServiceMessageEvent(object sender, DacMessageEventArgs e)
        {
            Console.WriteLine(string.Format("Message Type:{0} Prefix:{1} Number:{2} Message:{3}", e.Message.MessageType,
                                            e.Message.Prefix, e.Message.Number, e.Message.Message));
        }

        private static void receiveDacServiceProgessEvent(object sender, DacProgressEventArgs e)
        {
            Console.WriteLine(string.Format("Progress Event:{0} Progrss Status:{1}", e.Message, e.Status));
        }
    }
}