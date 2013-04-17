using Quartz;
using Quartz.Impl;

namespace AzureDBackup
{
    public class BackupHandler
    {
        private readonly IScheduler sched;
        private readonly ISchedulerFactory schedFact;

        public BackupHandler()
        {
            schedFact = new StdSchedulerFactory();
            sched = schedFact.GetScheduler();
            sched.Start();
        }

        /// <summary>
        ///     Add a job to Quartz
        /// </summary>
        /// <param name="bctx">All settings for a job</param>
        public void AddJob(BackupContext bctx)
        {
            IJobDetail job = JobBuilder.Create<DbBackup>()
                                       .WithIdentity(bctx.Id, "group1") // needs to be unique
                                       .Build();
            job.JobDataMap.Add("bctx", bctx);

            ITrigger trigger = TriggerBuilder.Create()
                                             .WithCronSchedule(bctx.CronSchedule) // Cron schedule
//                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Second))
//                .WithDailyTimeIntervalSchedule(x => x.WithIntervalInSeconds(2))
//                                .WithSimpleSchedule(x => x.WithIntervalInSeconds(1))
                                             .Build();


            sched.ScheduleJob(job, trigger);
        }
    }
}