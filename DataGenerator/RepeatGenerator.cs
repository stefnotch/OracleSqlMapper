using System.Collections.Generic;

namespace DataGenerator
{
    public class RepeatGenerator<T> : IGenerator<T>
    {
        private readonly IGenerator<T> _generator;
        private readonly int _repeatEachElementCount;

        public RepeatGenerator(IGenerator<T> generator, int repeatEachElementCount)
        {
            _generator = generator;
            _repeatEachElementCount = repeatEachElementCount;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = _generator.GetEnumerator();
            int repeatCounter = _repeatEachElementCount;
            while (true)
            {
                repeatCounter++;
                if (repeatCounter >= _repeatEachElementCount)
                {
                    enumerator.MoveNext();
                    repeatCounter = 0;
                }
                yield return enumerator.Current;
            }
        }
    }
}