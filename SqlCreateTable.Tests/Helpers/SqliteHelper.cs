using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace SqlCreateTable.Tests.Helpers
{
    public static class SqliteHelper
    {
        private const string DbDirectory = "./DB";

        private static string _dataSource;

        public static SqliteConnection Connection { get; private set; }

        public static void OpenConnection(string fileName)
        {
            PrepareDataSource(fileName);

            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = _dataSource,
                Mode = SqliteOpenMode.ReadWriteCreate,
                ForeignKeys = true,
            }.ConnectionString;

            Connection = new SqliteConnection(connectionString);
            Connection.Open();
        }

        public static void OpenConnection()
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                Mode = SqliteOpenMode.Memory,
                ForeignKeys = true,

            }.ConnectionString;

            Connection = new SqliteConnection(connectionString);
            Connection.Open();
        }

        public static void CloseConnection(bool deleteDataSource = true)
        {
            if (Connection.State is ConnectionState.Open)
                Connection.Close();
            if (deleteDataSource)
                DeleteDataSource();
        }

        private static void PrepareDataSource(string fileName)
        {
            Directory.CreateDirectory(DbDirectory);
            _dataSource = Path.Combine(DbDirectory, fileName);
            if (File.Exists(_dataSource))
                File.Delete(_dataSource);
        }

        private static void DeleteDataSource()
        {
            if (Directory.Exists(DbDirectory))
                Directory.Delete(DbDirectory, true);
            _dataSource = null;
        }
    }
}