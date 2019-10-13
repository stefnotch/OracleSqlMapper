using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlMapper.SqlObjects.Constraints
{
    public class ForeignKeyConstraint : Constraint
    {
        public ForeignKeyConstraint(Table table, string name, IEnumerable<TableColumn> columns, IEnumerable<TableColumn> referencedColumns)
            : base(table, name)
        {
            Columns.AddRange(columns);
            ReferencedColumns.AddRange(referencedColumns);
        }

        public ForeignKeyConstraint(Table table, string name, TableColumn column, TableColumn referencedColumn)
            : this(table, name, Enumerable.Repeat(column, 1), Enumerable.Repeat(referencedColumn, 1))
        {
        }

        public List<TableColumn> Columns { get; } = new List<TableColumn>();

        public List<TableColumn> ReferencedColumns { get; } = new List<TableColumn>();

        public override TableColumn SingleAffectedColumn => (Columns.Count == 1) ? Columns[0] : null;

        public override string ToString()
        {
            if (Columns.Count != ReferencedColumns.Count)
            {
                throw new Exception("Foreign key column counts don't match up");
            }

            return base.ToString() +
                (IsInlineSingleColumn ? "" : $"FOREIGN KEY ({string.Join(", ", Columns.Select(c => c.Name))}) ") +
                $"REFERENCES {ReferencedColumns[0].Table.SqlName}({string.Join(", ", ReferencedColumns.Select(c => c.Name))})";
        }
    }
}
