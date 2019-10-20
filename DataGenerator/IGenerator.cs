using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataGenerator
{
    public interface IGenerator : IEnumerable
    {

    }

    public interface IGenerator<out T> : IGenerator, IEnumerable<T>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public interface IIndexedGenerator<out T> : IGenerator<T>
    {
        T this[int index] { get; }
    }
}
