using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.SqlObjects
{
    public class Trigger : DatabaseObject
    {
        public string SqlCode { get; set; }
        public Trigger(string name) : base(name)
        {
        }

        public override string ToString()
        {
            return ToStringCreate(true);
        }

        public override string ToStringCreate(bool replace)
        {
            if (replace)
            {
                return $"CREATE OR REPLACE TRIGGER {SqlName}\n{SqlCode}\n/\n";
            }
            else
            {
                return $"CREATE TRIGGER {SqlName}\n{SqlCode}\n/\n";
            }
        }

        public override string ToStringAlter()
        {
            throw new NotImplementedException();
        }

        public override string ToStringDrop()
        {
            throw new NotImplementedException();
        }
    }
}
