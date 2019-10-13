using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlMapper.SqlPrimitives;

namespace SqlMapper.SqlObjects
{
    public class TableColumn : ICommentable
    {
        public readonly Table Table;
        public readonly string Name;
        public readonly Datatype Datatype;

        public TableColumn(Table table, string name, Datatype datatype)
        {
            if (name.Length > SqlUtils.MaxNameLength) throw new ArgumentException(nameof(name));
            Table = table;
            Name = name;
            Datatype = datatype;
        }

        public string Comment { get; set; }

        public string GetSqlComment(string spaces)
        {
            if (string.IsNullOrEmpty(Comment)) return "";

            string sqlComment = Comment.Trim('\n');
            string[] commentLines = sqlComment.Split('\n');
            return commentLines
                .Select(line => $"{spaces}-- {line}")
                .ToDelimitedString("\n");
        }
    }
}
