using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper
{
    public interface ICommentable
    {
        string Comment { get; set; }
        string GetSqlComment(string spaces);
    }
}
