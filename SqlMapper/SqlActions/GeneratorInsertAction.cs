using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator;

namespace SqlMapper.SqlObjects
{
    public class GeneratorInsertAction : InsertAction
    {
        public GeneratorInsertAction(Table table) : base(table)
        {
        }

        public Dictionary<TableColumn, IGenerator> Generators { get; } = new Dictionary<TableColumn, IGenerator>();

        public void Generate(int count)
        {
            InsertCount = count;
        }

        public override string ToStringExecute()
        {
            string sqlColumnNames = Generators.Keys
                .Select(col => col.SqlName)
                .ToDelimitedString(", ");

            var valueEnumerators = Generators.Values
                    .Select(gen => gen.GetEnumerator())
                    .ToList();

            string insertSqlCode = $"INSERT INTO {Table.SqlName} ({sqlColumnNames}) VALUES ";

            // TODO: Optional PL/SQL Code generation
            string sqlCode = "";
            for (int i = 0; i < InsertCount; i++)
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
