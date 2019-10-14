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
            return ToStringDrop() + "\n" + ToStringCreate();
        }

        public string ToStringDrop()
        {
            return $"DROP SEQUENCE {SqlName};";
        }

        public string ToStringCreate()
        {
            string sqlCode = $"CREATE SEQUENCE {SqlName}";
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
    }
}
