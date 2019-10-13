using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute()
        {

        }
    }
}
