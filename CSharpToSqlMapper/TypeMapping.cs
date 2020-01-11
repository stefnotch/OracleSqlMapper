using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.SqlPrimitives;

namespace CSharpToSqlMapper
{
    internal class TypeMapping
    {
        public Type Type { get; }

        public Datatype SqlType { get; }

        protected TypeMapping(Type type, int? size = null)
        {
            Type = type;
            SqlType = GetSqlType(type, size);
        }

        public static TypeMapping From(Type type, int? size = null)
        {
            if ((Nullable.GetUnderlyingType(type) ?? type).IsEnum)
            {
                return new EnumTypeMapping(type);
            }
            else
            {
                return new TypeMapping(type, size);
            }
        }

        public static TypeMapping From<T>(int? size = null)
        {
            return From(typeof(T), size);
        }

        private Datatype GetSqlType(Type type, int? size)
        {
            // TODO: Use the size
            type = Nullable.GetUnderlyingType(type) ?? type;

            // TODO: BigInteger, etc.
            if (type == typeof(short))
            {
                return new Datatype("NUMBER(5)");
            }
            else if (type == typeof(int))
            {
                return new Datatype("NUMBER(10)"); // Or INTEGER?
            }
            else if (type == typeof(long))
            {
                return new Datatype("NUMBER(20)");
            }
            else if (type == typeof(float))
            {
                return new Datatype("NUMBER(30, 10)");
            }
            else if (type == typeof(double))
            {
                return new Datatype("NUMBER(60, 15)");
            }
            else if (type == typeof(string))
            {
                return new Datatype($"VARCHAR2({size ?? 100})");
            }
            else if (type == typeof(bool))
            {
                // https://stackoverflow.com/questions/3726758/is-there-any-boolean-type-in-oracle-databases
                return new Datatype($"NUMBER(1)");
            }
            else if (type.IsEnum)
            {
                return new Datatype("VARCHAR2(4)");
            }
            else if (type == typeof(DateTime))
            {
                return new Datatype("TIMESTAMP");
            }
            else
            {
                return Datatype.Reference;
            }
        }

        public virtual string ValueToSql(object value)
        {
            return value switch
            {
                string v => $"'{v}'",
                short v => v.ToString(),
                int v => v.ToString(),
                long v => v.ToString(),
                float v => v.ToString(),
                double v => v.ToString(),
                bool v => v ? "1" : "0",
                DateTime v => $"TO_TIMESTAMP('{v.Year:0000}-{v.Month:00}-{v.Day:00}" +
                    $" {v.Hour:00}:{v.Minute:00}:{v.Second:00}.{v.Millisecond:000}', 'YYYY-MM-DD HH24:MI:SS.FF3')",
                null => "NULL",
                //TODO: Object (fk)
                _ => throw new NotImplementedException()
            };
        }
    }
}
