using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SqlCreateTable.Tests.Models;
using SqlCreateTable.Tests.Parsers;

namespace SqlCreateTable.Tests.Helpers
{
    public static class CreateTableHelper
    {
        private static readonly Regex CreateRegExp = new Regex(@"^\s*CREATE\sTABLE");
        private static readonly Regex PrimaryKeyRegExp = new Regex(@"\s+PRIMARY\s+KEY");
        private static readonly Regex ForeignKeyRegExp = new Regex(@"\s+FOREIGN\s+KEY");
        private static readonly Regex UniqueKeyRegExp = new Regex(@"\s+UNIQUE");
        private static Dictionary<string, DbTable> _tableMap;
        
        public static void ValidateConstrainKeyIntegrity(string query, IEnumerable<string> queries)
        {
            LoadTables(queries);
            var table = QueryParser.ParseTable(query);
            foreach(var fk in table.ForeignKeys)
            {
                var precedence = _tableMap[table.TableName].Precedence;
                var refTable = _tableMap[fk.RefTable];
                if (precedence < refTable.Precedence)
                    throw new ArgumentException($"Table '{fk.RefTable}' must be created before table '{table.TableName}'.");
                if (!refTable.ColumnList.ContainsKey(fk.RefColumn))
                    throw new ArgumentException($"Foreign key '{fk.LocalColumn}' in table '{table.TableName}' REFERENCES not existing column '{fk.RefColumn}' in table '{fk.RefTable}'.");
                if (refTable.ColumnList[fk.RefColumn] != table.ColumnList[fk.LocalColumn])
                    throw new ArgumentException($"Column '{table.TableName}.{fk.LocalColumn}' and '{refTable.TableName}.{fk.RefColumn}' have different types.");
            }
        }
        
        public static IEnumerable<string> GetOnlyQueriesWithForeignKeys(IEnumerable<string> queries) => queries.Where(ContainsForeignKey);
        public static bool ContainsCreateTableStatement(string query) => CreateRegExp.IsMatch(query.ToUpper());
        public static bool ContainsPrimaryKey(string query) => PrimaryKeyRegExp.IsMatch(query.ToUpper());
        public static bool ContainsForeignKey(string query) => ForeignKeyRegExp.IsMatch(query.ToUpper());
        public static bool ContainsUniqueKey(string query) => UniqueKeyRegExp.IsMatch(query.ToUpper());

        private static void LoadTables(IEnumerable<string> queries)
        {
            if (_tableMap is { }) return;
            _tableMap = new Dictionary<string, DbTable>();
            for (var i = 0; i < queries.Count(); i++)
            {
                var query = queries.ElementAt(i);
                var table = QueryParser.ParseTable(query);
                table.Precedence = i;
                _tableMap.Add(table.TableName, table);
            }
        }
    }
}