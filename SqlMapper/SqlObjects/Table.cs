using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SqlMapper.Attributes;
using SqlMapper.SqlObjects.Constraints;
using SqlMapper.SqlPrimitives;

namespace SqlMapper.SqlObjects
{
    public class Table : DatabaseObject
    {
        public List<TableColumn> Columns { get; } = new List<TableColumn>();
        public List<Constraint> Constraints { get; } = new List<Constraint>();

        public Table(string name) : base(name)
        {
        }

        public override string ToString()
        {
            return ToStringCreate(true) + "\n" + ToStringAlter();
        }

        public override string ToStringDrop()
        {
            string dropSafe = $@"BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE {SqlName} CASCADE CONSTRAINTS';
EXCEPTION
   WHEN OTHERS THEN
      IF SQLCODE != -942 THEN
         RAISE;
      ELSE
         DBMS_OUTPUT.PUT_LINE('Table {SqlName} skipped');
      END IF;
END;
/";
            return dropSafe;
        }

        public override string ToStringCreate(bool replace)
        {
            var singleColumnConstraints = Constraints
                            .Where(c => c.IsInline && c.SingleAffectedColumn != null)
                            .ToList();

            string multiColumnConstraints = Constraints
                            .Where(c => c.IsInline && c.SingleAffectedColumn == null)
                            .Select(con => $"    {con.ToString()}")
                            .ToDelimitedString(",\n");

            return (replace ? ToStringDrop() + "\n" : "") +
                $"{GetSqlComment("")}\n" +
                $"CREATE TABLE {SqlName} (\n" +
                Columns
                    .Select(column =>
                        $"{column.GetSqlComment("    ")}\n" +
                        (string.IsNullOrEmpty(column.Datatype.Comment) ? "" : $"{column.Datatype.GetSqlComment("    ")}\n") +
                        $"    {column.SqlName} {column.Datatype.Name}" +
                        singleColumnConstraints
                            .Where(con => con.SingleAffectedColumn == column)
                            .Select(con => " " + con.ToString())
                            .ToDelimitedString("")
                    )
                    .ToDelimitedString(",\n") +
                (string.IsNullOrEmpty(multiColumnConstraints) ? "" : $",\n{multiColumnConstraints}") +
                $"\n);";
        }

        public override string ToStringAlter()
        {
            string sqlCode = "";
            foreach (var constraint in Constraints)
            {
                if (constraint.IsInline) continue;

                sqlCode += $"ALTER TABLE {SqlName} ADD {constraint.ToString()};\n";
            }

            return sqlCode;
        }

        public override bool HasVariableWithName(string variableName)
        {
            return Columns.Any(col => col.SqlName == variableName);
        }
    }
}
