using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.SqlObjects
{
    public interface ICommentable
    {
        string Comment { get; set; }
        string GetSqlComment(string spaces);
    }
}
