using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlMapper.SqlObjects.Constraints
{
    /// <remarks>
    /// Must be defined inline
    /// </remarks>
    public class NotNullConstraint : Constraint
    {

        public NotNullConstraint(Table table, string name, TableColumn column) : base(table, name)
        {
            Column = column;
            IsInline = true;
        }

        public TableColumn Column { get; }

        public override TableColumn SingleAffectedColumn => Column;

        public override string ToString()
        {
            if (IsInlineSingleColumn)
            {
                return base.ToString() + "NOT NULL";
            }
            else
            {
                throw new Exception("Not Null Constraint has to be inline");
            }
        }
    }
}
