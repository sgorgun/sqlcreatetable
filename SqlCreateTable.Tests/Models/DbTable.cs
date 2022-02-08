using System;
using System.Collections.Generic;

namespace SqlCreateTable.Tests.Models
{
    public class DbTable
    {
        public string TableName { get; set; }
        public Dictionary<string, string> ColumnList { get; }
        public List<DbTableForeignKey> ForeignKeys { get; }
        public int Precedence { get; set; }
        
        public DbTable()
        {
            ColumnList = new Dictionary<string, string>(10);
            ForeignKeys = new List<DbTableForeignKey>();
        }

        public override string ToString()
        {
            return $"{TableName}:"+string.Join(',',ColumnList)+";"+string.Join(',',ForeignKeys);
        }
    }
}