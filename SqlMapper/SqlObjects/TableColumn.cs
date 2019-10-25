using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator;
using SqlMapper.SqlPrimitives;

namespace SqlMapper.SqlObjects
{
    public class TableColumn : ICommentable
    {
        public readonly Table Table;
        public readonly string Name;
        public readonly string SqlName;
        public readonly Datatype Datatype;

        public TableColumn(Table table, string name, Datatype datatype)
        {
            Table = table;
            Name = name;
            Datatype = datatype;

            SqlName = GetSqlName(name);
        }

        private string GetSqlName(string name)
        {
            string sqlName = SqlUtils.ToSqlName(name);
            int nameLength = sqlName.Length;
            int counter = 0;
            while (Table.Columns.Any(col => col.SqlName == sqlName))
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
            if (string.IsNullOrEmpty(Comment)) return "";

            string sqlComment = Comment.Trim('\n');
            string[] commentLines = sqlComment.Split('\n');
            return commentLines
                .Select(line => $"{spaces}-- {line}")
                .ToDelimitedString("\n");
        }
    }
}
