using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlMapper.SqlObjects.Constraints
{
    /// <remarks>
    /// Only one per table
    /// </remarks>
    public class PrimaryKeyConstraint : Constraint
    {
        public PrimaryKeyConstraint(Table table, string name, IEnumerable<TableColumn> columns) : base(table, name)
        {
            Columns.AddRange(columns);
        }

        public PrimaryKeyConstraint(Table table, string name, TableColumn column) : this(table, name, Enumerable.Repeat(column, 1))
        {
        }

        public List<TableColumn> Columns { get; } = new List<TableColumn>();

        public override TableColumn SingleAffectedColumn => (Columns.Count == 1) ? Columns[0] : null;

        public override string ToString()
        {
            return base.ToString() +
                (IsInlineSingleColumn ? "PRIMARY KEY" : $"PRIMARY KEY ({string.Join(", ", Columns.Select(c => c.SqlName))})");
        }
    }
}
