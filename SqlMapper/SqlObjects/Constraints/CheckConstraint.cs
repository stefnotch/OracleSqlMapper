using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.SqlObjects.Constraints
{
    public class CheckConstraint : Constraint
    {
        // TODO: CheckAttribute
        public CheckConstraint(Table table, string name, string sqlCode) : base(table, name)
        {
            SqlCode = sqlCode;
        }

        public string SqlCode { get; }

        public override TableColumn SingleAffectedColumn => null;

        public override string ToString()
        {
            return base.ToString() + $"CHECK ({SqlCode})";
        }
    }
}
