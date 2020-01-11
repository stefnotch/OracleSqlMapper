using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlMapper.SqlPrimitives;

namespace SqlMapper.PlsqlObjects
{
    public class Function : DatabaseObject
    {
        public List<Parameter> Parameters { get; } = new List<Parameter>();

        public Datatype ReturnType { get; set; }

        public string Body { get; set; }

        public Function(string name) : base(name)
        {
        }

        public override bool HasVariableWithName(string variableName)
        {
            return Parameters.Any(p => p.SqlName == variableName);
        }

        public override string ToString()
        {
            return $"FUNCTION {SqlName}\n" +
                (Parameters.Count > 0 ? $"({Parameters.Select(p => p.ToString()).ToDelimitedString(", ")})\n" : "") +
                $"RETURN {ReturnType}\n" +
                "IS\n" +
                "BEGIN\n" +
                Body +
                "EXCEPTION\n" +
                "    WHEN OTHERS THEN\n" +
                "        DBMS_OUTPUT.PUT_LINE(SQLCODE || SQLERRM);\n" +
                // TODO: Functions should be pure? (So no rolling back?)
                "END";
        }

        public override string ToStringAlter()
        {
            throw new NotImplementedException();
        }

        public override string ToStringCreate(bool replace)
        {
            if (replace)
            {
                return $"CREATE OR REPLACE {ToString()};";
            }
            else
            {
                return $"CREATE {ToString()};";
            }
        }

        public override string ToStringDrop()
        {
            return $"DROP FUNCTION {SqlName};";
        }
    }
}
