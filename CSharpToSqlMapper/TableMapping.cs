using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.SqlObjects;

namespace CSharpToSqlMapper
{
    internal class TableMapping
    {
        public TableMapping(Type type, Table table)
        {
            Type = type;
            Table = table;
        }

        public bool DatabaseGenerated { get; set; }
        public Sequence PrimaryKeySequence { get; set; }
        public Type Type { get; }
        public Table Table { get; }
        public Dictionary<string, TableColumn> Columns { get; } = new Dictionary<string, TableColumn>();
    }
}
