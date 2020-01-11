using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.SqlObjects
{
    public abstract class InsertAction : DatabaseAction
    {
        public InsertAction(Table table)
        {
            Table = table;
        }

        public Table Table { get; }

        public int InsertCount { get; protected set; } = 1;
    }
}
