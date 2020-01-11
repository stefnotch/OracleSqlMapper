using System;
using System.Reflection;
using SqlMapper.Attributes;

namespace SqlMapper.SqlPrimitives
{
    public class Datatype : IEquatable<Datatype>, ICommentable
    {
        private const string ReferenceName = "NUMBER(10) /*REFERENCE*/";
        public string Name { get; }
        public string Comment { get; set; }

        public Datatype(string name)
        {
            Name = name;
        }

        public static Datatype Reference => new Datatype(ReferenceName);

        public bool IsReference => Name == ReferenceName;

        public string GetSqlComment(string spaces)
        {
            return SqlUtils.ToSqlComment(Comment, spaces);
        }

        public override string ToString()
        {
            return Name;
        }

        #region Struct Comparison
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
        #endregion Struct Comparison
    }
}