using SRMAgreement;
using Microsoft.Data.Sqlite;

namespace SRMAgreement.Data_Base
{
    public class DataBase
    {
        private readonly string _connectionString;
        private readonly string _databaseFilePath;
        private readonly string _backupDirectory;

        public DataBase(IConfiguration configuration, IWebHostEnvironment env)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _databaseFilePath = GetDatabaseFilePath(_connectionString);
            _backupDirectory = @"C:\work\DataBase\Backup\";

            EnsureBackupDirectoryExists();
        }

        private string GetDatabaseFilePath(string connectionString)
        {
            var builder = new SqliteConnectionStringBuilder(connectionString);
            return builder.DataSource;
        }

        private void EnsureBackupDirectoryExists()
        {
            if (!Directory.Exists(_backupDirectory))
            {
                Directory.CreateDirectory(_backupDirectory);
            }
        }

        public string BackupDatabase()
        {
            var backupFilePath = Path.Combine(_backupDirectory, $"database_backup_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.sqlite");

            if (File.Exists(_databaseFilePath))
            {
                File.Copy(_databaseFilePath, backupFilePath, true);
                return backupFilePath;
            }
            else
            {
                throw new FileNotFoundException("Database file not found.");
            }
        }
        public void DataBaseBackUpTime()
        {
            
        }
    }
}
