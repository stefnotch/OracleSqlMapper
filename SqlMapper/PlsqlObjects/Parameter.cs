using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.SqlPrimitives;

namespace SqlMapper.PlsqlObjects
{
    public class Parameter : DatabaseObjectVariable<Function>
    {
        public Parameter(Function databaseObject, string name, Datatype datatype) : base(databaseObject, name, datatype)
        {
        }

        public override string ToString()
        {
            return $"{SqlName} {Datatype}";
        }
    }
}
