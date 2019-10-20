using System.Collections.Generic;

namespace DataGenerator
{
    public class SequentialFromGenerator<T> : IGenerator<T>
    {
        private readonly IEnumerable<T> _enumerable;

        public SequentialFromGenerator(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }
        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = _enumerable.GetEnumerator();
            while (true)
            {
                if (!enumerator.MoveNext())
                {
                    enumerator = _enumerable.GetEnumerator();
                    enumerator.MoveNext();
                }
                yield return enumerator.Current;
            }
        }
    }
}