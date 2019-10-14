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
            return ToStringCreateOrReplace();
        }

        public string ToStringCreateOrReplace()
        {
            return $"CREATE OR REPLACE TRIGGER {SqlName}\n{SqlCode}\n/\n";
        }
    }
}
