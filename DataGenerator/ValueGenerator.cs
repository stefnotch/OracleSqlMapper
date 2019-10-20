using System;
using System.Collections.Generic;
using System.Text;

namespace DataGenerator
{
    // TODO: Auto cast strings https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators
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
