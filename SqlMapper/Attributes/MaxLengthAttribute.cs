using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMapper.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class MaxLengthAttribute : Attribute
    {
        public readonly int Length;

        public MaxLengthAttribute(int length)
        {
            this.Length = length;
        }
    }
}
