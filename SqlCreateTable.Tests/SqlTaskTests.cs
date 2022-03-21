using System;
using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.Data.Sqlite;
using System.IO;
using AutocodeDB.Helpers;

namespace SqlCreateTable.Tests
{
    [TestFixture]
    public class SqlTaskTests
    {
        private const int CreateQueriesCount = 7;
        private const string CreateQueriesFileName = "create.sql";
        private static readonly string CreateQueriesFile = SqlTask.GetQueriesFullPath(CreateQueriesFileName);
        private static readonly string[] CreateQueries = QueryHelper.GetQueries(CreateQueriesFile);
        private static readonly IEnumerable<string> CreateQueriesWithForeignKeys = 
            CreateTableHelper.GetOnlyQueriesWithForeignKeys(CreateQueries);

        [OneTimeSetUp]
        public void Setup()
        {
            SqliteHelper.OpenConnection();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            SqliteHelper.CloseConnection();
        }

        #region Local Tests

        [Test]
        public void FileWithQueries_Exists()
        {
            var actual = File.Exists(CreateQueriesFile);
            var message = $"Couldn't find the '{CreateQueriesFileName}' file.";
            Assert.IsTrue(actual, message);
        }

        [Test]
        public void FileWithQueries_NotEmpty()
        {
            var message = $"The file '{CreateQueriesFileName}' contains no entries.";
            Assert.IsNotEmpty(CreateQueries, message);
        }

        [Test]
        public void FileWithQueries_QueriesCount()
        {
            var message = $"There should be at least {CreateQueriesCount} queries in the '{CreateQueriesFileName}' file.";
            Assert.GreaterOrEqual(CreateQueries.Length, CreateQueriesCount, message);
        }

        [TestCaseSource(nameof(CreateQueries))]
        public void CreateTableQueryString_ContainsCorrectCreateTableStatement(string query)
        {
            var actual = CreateTableHelper.ContainsCreateTableStatement(query);
            var message = QueryHelper.ComposeErrorMessage(query, "The query doesn't contain 'CREATE TABLE' statement.");
            Assert.IsTrue(actual, message);
        }

        [TestCaseSource(nameof(CreateQueries))]
        public void AllCreateTableQueries_ExecutesSuccessfully(string query)
        {
            try
            {
                var command = new SqliteCommand(query, SqliteHelper.Connection);
                command.ExecuteNonQuery();
            }
            catch (SqliteException exception)
            {
                var message = QueryHelper.ComposeErrorMessage(query, exception, "Query execution caused an exception.");
                Assert.Fail(message);
            }
        }

        [TestCaseSource(nameof(CreateQueriesWithForeignKeys))]
        public void CreateQueriesWithForeignKeys_ForeignKeyReferencesExist(string query)
        {
            try
            {
                CreateTableHelper.ValidateConstrainKeyIntegrity(query, CreateQueries);
            }
            catch (Exception exception)
            {
                var message = QueryHelper.ComposeErrorMessage(query, exception);
                Assert.Fail(message); 
            }
        }
        [TestCaseSource(nameof(CreateQueriesWithForeignKeys))]
        public void CreateQueriesWithForeignKeys_OnDeleteCascadeNotExist(string query)
        {
            var message = QueryHelper.ComposeErrorMessage(query, "Each table should not contain ON DELETE CASCADE.");
            var actual = CreateTableHelper.ContainsOnDeleteCascade(query);
            Assert.IsFalse(actual, message);
        }
        #endregion
    }
}
