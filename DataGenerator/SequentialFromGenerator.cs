using System.Collections.Generic;
using System.Linq;

namespace DataGenerator
{
    /// <summary>
    /// Gets an element from each generator in sequential, cycling order
    /// </summary>
    public class SequentialFromGenerator<T> : IGenerator<T>
    {
        private readonly IGenerator<T>[] _generators;

        public SequentialFromGenerator(IGenerator<T>[] generators)
        {
            _generators = generators;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int index = 0;
            var enumerators = _generators.Select(g => g.GetEnumerator()).ToList();
            while (true)
            {
                enumerators[index].MoveNext();
                yield return enumerators[index].Current;

                index = (index + 1) % _generators.Length;
            }
        }
    }
}