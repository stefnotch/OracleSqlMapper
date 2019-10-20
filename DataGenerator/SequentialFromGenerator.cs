using System.Collections.Generic;

namespace DataGenerator
{
    public class SequentialFromGenerator<T> : IGenerator<T>
    {
        private IEnumerable<T> _enumerable;

        public SequentialFromGenerator(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }
        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = _enumerable.GetEnumerator();
            while (true)
            {
                enumerator.MoveNext();
                yield return enumerator.Current;
            }
        }
    }
}