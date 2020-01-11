using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlMapper;

namespace CSharpToSqlMapper
{
    internal class EnumTypeMapping : TypeMapping
    {
        public List<string> EnumValueNames { get; }
        public List<string> EnumSqlNames { get; }

        public EnumTypeMapping(Type type) : base(type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (!type.IsEnum)
            {
                throw new ArgumentException("Not an enum", nameof(type));
            }

            EnumValueNames = Enum.GetNames(type)
                            .Select(name => name.Trim())
                            .OrderBy(name => name)
                            .ToList();

            EnumSqlNames = EnumValueNames
                .Select(name => SqlUtils.PascalCaseToSnakeCase(name))
                .Select(name =>
                {
                    string[] nameParts = name.Split("_");
                    if (nameParts.Length <= 1)
                    {
                        return nameParts[0].Substring(0, 4);
                    }
                    else if (nameParts.Length == 2)
                    {
                        return $"{nameParts[0].Substring(0, 2)}{nameParts[1].Substring(0, 2)}";
                    }
                    else if (nameParts.Length == 3)
                    {
                        return $"{nameParts[0].Substring(0, 2)}{nameParts[1].Substring(0, 1)}{nameParts[2].Substring(0, 1)}";
                    }
                    else
                    {
                        return $"{nameParts[0][0]}{nameParts[1][0]}{nameParts[2][0]}{nameParts[3][0]}";
                    }
                })
                .ToList();

            for (int i = 0; i < EnumSqlNames.Count; i++)
            {
                int nameLength = EnumSqlNames[i].Length;
                int counter = 0;
                while (EnumSqlNames.Take(i).Contains(EnumSqlNames[i]))
                {
                    string counterText = counter + "";
                    EnumSqlNames[i] = EnumSqlNames[i].Substring(0, nameLength - counterText.Length) + counterText;
                    counter++;
                }
            }

            SqlType.Comment += EnumSqlNames
                        .Select((sqlName, index) => $"{sqlName}: {EnumValueNames[index]}")
                        .ToDelimitedString("\n");
        }

        public override string ValueToSql(object value)
        {
            if (value == null)
            {
                return base.ValueToSql(value);
            }

            string name = Enum.GetName(Nullable.GetUnderlyingType(Type) ?? Type, value);
            int index = EnumValueNames.IndexOf(name);
            return $"'{EnumSqlNames[index]}'";
        }
    }
}
