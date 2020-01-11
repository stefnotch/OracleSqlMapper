using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.PlsqlObjects
{
    public class Package : DatabaseObject
    {
        public Package(string name) : base(name)
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override string ToStringAlter()
        {
            throw new NotImplementedException();
        }

        public override string ToStringCreate(bool replace)
        {
            throw new NotImplementedException();
        }

        public override string ToStringDrop()
        {
            return $"DROP PACKAGE {SqlName};";
        }
    }
}
