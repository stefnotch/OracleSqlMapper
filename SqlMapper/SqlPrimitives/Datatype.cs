using System;
using System.Reflection;
using SqlMapper.Attributes;

namespace SqlMapper.SqlPrimitives
{
    public readonly struct Datatype : IEquatable<Datatype>
    {
        public readonly string Name;
        public Datatype(string name)
        {
            Name = name;
        }

        public static Datatype FromPropertyInfo(PropertyInfo prop, out bool isForeignKey)
        {
            isForeignKey = false;
            // TODO: BigInteger, etc.
            Type propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (propertyType == typeof(short))
            {
                return new Datatype("NUMBER(5)");
            }
            else if (propertyType == typeof(int))
            {
                return new Datatype("NUMBER(10)"); // Or INTEGER?
            }
            else if (propertyType == typeof(long))
            {
                return new Datatype("NUMBER(20)");
            }
            else if (propertyType == typeof(float))
            {
                return new Datatype("NUMBER(30, 10)");
            }
            else if (propertyType == typeof(double))
            {
                return new Datatype("NUMBER(60, 15)");
            }
            else if (propertyType == typeof(string))
            {
                int size = prop.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? 40;
                return new Datatype($"VARCHAR2({size})");
            }
            else if (propertyType == typeof(bool))
            {
                // https://stackoverflow.com/questions/3726758/is-there-any-boolean-type-in-oracle-databases
                return new Datatype($"NUMBER(1)");
            }
            else if (propertyType.IsEnum)
            {
                return new Datatype("VARCHAR2(4)");
            }
            else if (propertyType == typeof(DateTime))
            {
                return new Datatype("TIMESTAMP");
            }
            else
            {
                // ID
                isForeignKey = true;
                return new Datatype("NUMBER(10)");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Datatype datatype && Equals(datatype);
        }

        public bool Equals(Datatype other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public static bool operator ==(Datatype left, Datatype right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Datatype left, Datatype right)
        {
            return !(left == right);
        }
    }
}