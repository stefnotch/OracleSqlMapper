using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public class RandomIntGenerator : IGenerator<int>
    {
        private readonly int _fromValue;
        private readonly int _toValue;
        private static readonly Random _rng = new Random();
        public RandomIntGenerator(int fromValue, int toValue)
        {
            _fromValue = fromValue;
            _toValue = toValue;
        }

        public IEnumerator<int> GetEnumerator()
        {
            while (true)
            {
                yield return _rng.Next(_fromValue, _toValue);
            }
        }
    }
}