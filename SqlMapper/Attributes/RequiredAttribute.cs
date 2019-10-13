using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.Attributes
{
    // TODO: Implement RequiredAttribute. Is equivalent to a NotNullAttribute.
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class RequiredAttribute : Attribute
    {
        public RequiredAttribute()
        {
        }
    }
}
