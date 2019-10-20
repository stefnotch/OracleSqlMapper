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
        public int InsertsCount { get; set; }

        public Table(string name) : base(name)
        {
        }

        public override string ToString()
        {
            return ToStringDrop() + "\n" + ToStringCreate() + "\n" + ToStringAlter();
        }

        public string ToStringDrop()
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

        public string ToStringCreate()
        {
            var singleColumnConstraints = Constraints
                            .Where(c => c.IsInline && c.SingleAffectedColumn != null)
                            .ToList();

            string multiColumnConstraints = Constraints
                            .Where(c => c.IsInline && c.SingleAffectedColumn == null)
                            .Select(con => $"    {con.ToString()}")
                            .ToDelimitedString(",\n");

            return $"{GetSqlComment("")}\n" +
                $"CREATE TABLE {SqlName} (\n" +
                Columns
                    .Select(column =>
                        $"{column.GetSqlComment("    ")}\n" +
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

        public string ToStringAlter()
        {
            string sqlCode = "";
            foreach (var constraint in Constraints)
            {
                if (constraint.IsInline) continue;

                sqlCode += $"ALTER TABLE {SqlName} ADD {constraint.ToString()};\n";
            }

            return sqlCode;
        }

        public string ToStringInsert()
        {
            string sqlColumnNames = Columns
                    .Where(col => col.Generator != null)
                    .Select(col => col.SqlName)
                    .ToDelimitedString(", ");

            var valueEnumerators = Columns
                    .Where(col => col.Generator != null)
                    .Select(col => SqlUtils.GeneratorToEnumerable(col.Generator).GetEnumerator())
                    .ToList();

            string insertSqlCode = $"INSERT INTO {SqlName} ({sqlColumnNames}) VALUES ";

            string sqlCode = "";
            for (int i = 0; i < InsertsCount; i++)
            {
                string insertValues = valueEnumerators
                    .Select(v => { v.MoveNext(); return v.Current; })
                    .ToDelimitedString(", ");

                sqlCode += $"{insertSqlCode} ({insertValues});\n";
            }

            return sqlCode;
        }
    }
}
