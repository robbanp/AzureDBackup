AzureDBackup
============

C# 4.5, Visual Studio 2012

A Database Backup Scheduler for Windows Azure SQL and Blob Storage you can use in your worker roles for example. It uses <a href="http://quartznet.sourceforge.net/">Quartz.NET</a> for scheduling (using Cron syntax) and <a href="http://www.microsoft.com/en-us/download/details.aspx?id=35756">Data-Tier Application Framework</a> for SQL backups.
It can send you e-mails when jobs are done and when they fail.

The generated database backups are pushed as .BACPAC files to your favourite Azure Blob Storage account in the container of your choice.

Dependencies
============

In order to get this working in a Worker Role you need to add a few DLL's to the References:
Add all the files in <a href="https://github.com/robbanp/AzureDBackup/tree/master/AzureDBackup/Assemblies">Assemblies</a> and set Copy Local to true (in visual studio).

```csharp
Microsoft.Data.Tools.Schema.Sql.dll
Microsoft.Data.Tools.Utilities.dll	
Microsoft.SqlServer.Dac.dll	
Microsoft.SqlServer.TransactSql.ScriptDom.dll	
Microsoft.SqlServer.TransactSql.dll	
Microsoft.SqlServer.Types.dll
```

How it works:

First you need to set up a BackupContext object that tell the service what to do when to what database. It contains connection strings, database names as well as E-mail notification settings.


Example: 

```csharp
var bctxDaily = new BackupContext
  {
    DatabaseConnection = ConfigurationManager.AppSettings["myConnextion"],
    DatabaseName = "scstest",
    StorageAccount = ConfigurationManager.AppSettings["blobStorage"],
    StorageContainer = "bbdbbackup-daily",
    CronSchedule = "0 0 4 1/1 * ? *", //daily at 04:00
    MailSetting = mailSettings,
    Id = "scshopdaily"
  };
```

And, this is a <a href="http://www.cronmaker.com/">great cron syntax tool</a>!

The example above sets the basic conf as well as a MailSetting object that looks like this:

```csharp
var mailSettings = new MailSettings
  {
    SMTPCredentials =
      new NetworkCredential(mycrazyemaillogin", "lepassword"),
    SMTPServer = "email-smtp.us-east-1.amazonaws.com",
    MailReceiver = "info@mydomain.se",
    SMTPServerPort = 587
  };
```

Then you need to create a BackupHandler to pass the context to:


Example: 
```csharp
var backups = new BackupHandler();
backups.AddJob(bctxDaily);
```

Jobs can be added for multi DB backups.

Apache License
Version 2.0

By Robert Pohl, robert@sugarcubesolutions.com - http://sugarcubesolutions.com