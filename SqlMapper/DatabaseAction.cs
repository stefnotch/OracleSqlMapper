using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlMapper
{
    /// <summary>
    /// An action like a insert statement
    /// </summary>
    public abstract class DatabaseAction : ICommentable
    {
        private Schema _schema;

        public string Tag { get; set; }

        protected DatabaseAction()
        {
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

        public abstract string ToStringExecute();
    }
}
