using System;
using System.Linq;
using System.Text.RegularExpressions;
using SqlCreateTable.Tests.Models;

namespace SqlCreateTable.Tests.Parsers
{
    public static class QueryParser
    {
        private static readonly Regex TableNameMatcher = new Regex(@"\s*CREATE\sTABLE\s(?<tblName>[A-Za-z_]*)\s*",
            RegexOptions.Compiled|RegexOptions.IgnoreCase); 
        private static readonly Regex ForeignKeyRegExp = new Regex(@"FOREIGN\s+KEY\s*[(](?<localId>[A-Za-z_]*)[)]\s*REFERENCES\s+(?<refTable>[A-Za-z_]*)\s*[(](?<refId>[A-Za-z_]*)[)]",
            RegexOptions.Compiled|RegexOptions.IgnoreCase);
        
        public static DbTable ParseTable(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length == 0)
                throw new ArgumentException("Incorrect create table query.");
            
            var matches=TableNameMatcher.Match(query);
            if(!matches.Success)
                throw new ArgumentException("Incorrect create table query.");
            
            var table = new DbTable
            {
                TableName = matches.Groups["tblName"].Value
            };

            var body = ParseTableBody(query);
            var tblColRow = body.Split(",");
            foreach (var rec in tblColRow)
            {
                var m = ForeignKeyRegExp.Match(rec);
                if (m.Success)
                {
                    table.ForeignKeys.Add(new DbTableForeignKey(m.Groups["localId"].Value,m.Groups["refTable"].Value,m.Groups["refId"].Value));
                }
                else
                {
                    var tmp = rec.Split(" ",StringSplitOptions.RemoveEmptyEntries).Where(s=>!string.IsNullOrWhiteSpace(s)).ToArray();
                    table.ColumnList.Add(tmp[0], tmp[1]);                        
                }
            }
            return table;
        }

        private static string ParseTableBody(string query)
        {
            var bodyBegin = query.IndexOf('(');
            var bodyEnd = query.LastIndexOf(')');
            if (bodyBegin < 0 || bodyEnd < 0)
                throw new ArgumentException("Incorrect create table query.");
            
            return query.Substring(bodyBegin + 1, bodyEnd - bodyBegin - 1);
        }
    }
}