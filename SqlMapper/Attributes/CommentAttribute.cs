using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public sealed class CommentAttribute : Attribute
    {
        public CommentAttribute(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; }
    }
}
