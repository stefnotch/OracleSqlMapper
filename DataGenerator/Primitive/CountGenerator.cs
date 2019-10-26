using System.Collections.Generic;

namespace DataGenerator
{
    public class CountGenerator : IGenerator<int>, IIndexedGenerator<int>
    {
        private readonly int _fromValue;
        private readonly int _stepSize;

        public CountGenerator(int fromValue, int stepSize)
        {
            _fromValue = fromValue;
            _stepSize = stepSize;
        }

        public int this[int index] => _fromValue + (index * _stepSize);

        public IEnumerator<int> GetEnumerator()
        {
            int currentValue = _fromValue;
            while (true)
            {
                yield return currentValue;
                currentValue += _stepSize;
            }
        }
    }
}