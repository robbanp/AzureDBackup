using Quartz;

namespace AzureDBackup
{
    public class BackupContext
    {
        /// <summary>
        /// Name of database
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Database connection string, ex: "Data Source=myserver.database.windows.net;Integrated Security=False;user id=myusername;password=mypassword;MultipleActiveResultSets=True"
        /// </summary>
        public string DatabaseConnection { get; set; }
        /// <summary>
        /// Azure Storage account, ex: "DefaultEndpointsProtocol=http;AccountName=mystorage;AccountKey=**mykey**"
        /// </summary>
        public string StorageAccount { get; set; }
        /// <summary>
        /// Azure storage container name
        /// </summary>
        public string StorageContainer { get; set; }
        /// <summary>
        /// Cron schedule data
        /// </summary>
        public string CronSchedule { get; set; }
        /// <summary>
        /// Settings for mail
        /// </summary>
        public MailSettings MailSetting { get; set; }
        /// <summary>
        /// Unique ID for job, ex:  "mydbbackup-daily"
        /// </summary>
        public string Id { get; set; }
    }
}