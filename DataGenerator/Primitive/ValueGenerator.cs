using System;
using System.Collections.Generic;
using System.Text;

namespace DataGenerator
{
    public class ValueGenerator<T> : IGenerator<T>
    {
        public ValueGenerator(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public IEnumerator<T> GetEnumerator()
        {
            while (true)
            {
                yield return Value;
            }
        }
    }
}
