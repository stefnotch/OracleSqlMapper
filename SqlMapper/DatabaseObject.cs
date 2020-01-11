using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlMapper
{
    /// <summary>
    /// An SQL object like a table, view, constraint, function, etc.
    /// https://docs.oracle.com/cd/B19306_01/server.102/b14200/sql_elements007.htm
    /// </summary>
    public abstract class DatabaseObject : ICommentable
    {
        private Schema _schema;

        public string Tag { get; set; }

        // TODO: Autogenerate names when not specified (especially for constraints)
        protected DatabaseObject(string name)
        {
            Name = name;
        }

        public Schema Schema
        {
            get => _schema;
            set
            {
                if (_schema != value)
                {
                    if (_schema == null)
                    {
                        _schema = value;
                        _schema.AddInternal(this);
                    }
                    else if (value == null)
                    {
                        _schema = null;
                        _schema.RemoveInternal(this);
                    }
                    else
                    {
                        throw new Exception("Object already attached to a different schema");
                    }
                }
            }
        }

        public string Comment { get; set; }
        public string GetSqlComment(string spaces)
        {
            return SqlUtils.ToSqlComment(Comment, spaces);
        }

        public string Name { get; }

        public string SqlName
        {
            get; internal set;
        }

        public virtual bool HasVariableWithName(string variableName)
        {
            return false;
        }

        public abstract string ToStringCreate(bool replace);
        public abstract string ToStringAlter();
        public abstract string ToStringDrop();
    }
}