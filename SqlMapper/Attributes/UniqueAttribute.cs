using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class UniqueAttribute : Attribute
    {
        public UniqueAttribute()
        {
        }
    }
}
