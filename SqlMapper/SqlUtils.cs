using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DataGenerator;
using SqlMapper.Attributes;
using SqlMapper.SqlPrimitives;

namespace SqlMapper
{
    public static class SqlUtils
    {
        public const int MaxNameLength = 30;

        public static string ToSqlName(string value)
        {
            string sqlName = PascalCaseToSnakeCase(value);

            if (value.Length > MaxNameLength)
            {
                sqlName = sqlName.Substring(0, MaxNameLength);
            }

            return sqlName;
        }

        public static string PascalCaseToSnakeCase(string value)
        {
            string sqlName = value.Trim();
            // PascalCase to snake_case
            for (int i = sqlName.Length - 1 - 1; i >= 0; i--)
            {
                if (char.IsLower(sqlName[i]) && char.IsUpper(sqlName[i + 1]))
                {
                    sqlName = sqlName.Insert(i + 1, "_");
                }
            }
            sqlName = sqlName.ToLower();
            return sqlName;
        }

        public static string ToDelimitedString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string ToDelimitedString<T>(this IEnumerable<T> source, char separator)
        {
            return string.Join(separator, source);
        }

        public static IEnumerable<string> GeneratorToEnumerable(IGenerator generator)
        {
            return generator switch
            {
                IGenerator<string> g => g.Select(v => $"'{v}'"),
                IGenerator<short> g => g.Select(v => v.ToString()),
                IGenerator<int> g => g.Select(v => v.ToString()),
                IGenerator<long> g => g.Select(v => v.ToString()),
                IGenerator<float> g => g.Select(v => v.ToString()),
                IGenerator<double> g => g.Select(v => v.ToString()),
                IGenerator<bool> g => g.Select(v => v ? "1" : "0"),
                // TODO: Enum (varchar2(4)), DateTime (timestamp), Object (fk)
                _ => throw new NotImplementedException()
            };
        }
    }
}
