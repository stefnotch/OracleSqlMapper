using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
    }
}
