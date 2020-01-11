using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.SqlObjects
{
    public class Sequence : DatabaseObject
    {
        public Sequence(string name) : base(name)
        {
        }

        public int? IncrementBy { get; set; }
        public int? StartWith { get; set; }

        public override string ToString()
        {
            return ToStringCreate(true);
        }

        public override string ToStringDrop()
        {
            return $"DROP SEQUENCE {SqlName};";
        }

        public override string ToStringCreate(bool replace)
        {
            string sqlCode = "";
            if (replace)
            {
                sqlCode += ToStringDrop() + "\n";
            }
            sqlCode += $"CREATE SEQUENCE {SqlName}";
            if (StartWith.HasValue)
            {
                sqlCode += $" START WITH {StartWith.Value}";
            }
            if (IncrementBy.HasValue)
            {
                sqlCode += $" INCREMENT BY {IncrementBy.Value}";
            }
            return $"{sqlCode};";
        }

        public override string ToStringAlter()
        {
            throw new NotImplementedException();
        }
    }
}
