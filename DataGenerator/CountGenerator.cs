using System.Collections.Generic;

namespace DataGenerator
{
    public class CountGenerator : IGenerator<int>
    {
        private readonly int _fromValue;
        private readonly int _stepSize;
        private int _currentValue;

        public CountGenerator(int fromValue, int stepSize)
        {
            _fromValue = fromValue;
            _stepSize = stepSize;
            _currentValue = _fromValue;
        }

        public IEnumerator<int> GetEnumerator()
        {
            while (true)
            {
                yield return _currentValue;
                _currentValue += _stepSize;
            }
        }
    }
}