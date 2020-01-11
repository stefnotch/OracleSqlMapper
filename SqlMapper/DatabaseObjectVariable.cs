using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlMapper.SqlPrimitives;

namespace SqlMapper
{
    public class DatabaseObjectVariable<T> : ICommentable where T : DatabaseObject
    {
        public readonly string Name;
        public readonly string SqlName;
        public readonly Datatype Datatype;
        public readonly T DatabaseObject;

        public DatabaseObjectVariable(T databaseObject, string name, Datatype datatype)
        {
            DatabaseObject = databaseObject;
            Name = name;
            Datatype = datatype;

            SqlName = GetSqlName(name);
        }

        private string GetSqlName(string name)
        {
            string sqlName = SqlUtils.ToSqlName(name);
            int nameLength = sqlName.Length;
            int counter = 0;
            while (DatabaseObject.HasVariableWithName(sqlName))
            {
                string counterText = counter + "";
                sqlName = sqlName.Substring(0, nameLength - counterText.Length) + counterText;
                counter++;
            }
            return sqlName;
        }

        public string Comment { get; set; }

        public string GetSqlComment(string spaces)
        {
            return SqlUtils.ToSqlComment(Comment, spaces);
        }
    }
}
