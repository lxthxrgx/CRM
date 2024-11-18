using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SRMAgreement.Data_Base
{
    public class DatabaseBackupService : IHostedService, IDisposable
    {
        private readonly ILogger<DatabaseBackupService> _logger;
        private readonly DataBase _database;
        private Timer _timer;

        public DatabaseBackupService(ILogger<DatabaseBackupService> logger, DataBase database)
        {
            _logger = logger;
            _database = database;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database Backup Service is starting.");
            _timer = new Timer(BackupDatabase, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));

            return Task.CompletedTask;
        }

        private void BackupDatabase(object state)
        {
            try
            {
                var backupFilePath = _database.BackupDatabase();
                _logger.LogInformation($"Database backup created at {backupFilePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating database backup.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database Backup Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
