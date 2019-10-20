using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public class RandomIntGenerator : IGenerator<int>
    {
        private readonly int _fromValue;
        private readonly int _toValue;
        private readonly int _randomSeed;
        private static readonly Random _seedRng = new Random();
        public RandomIntGenerator(int fromValue, int toValue)
        {
            _fromValue = fromValue;
            _toValue = toValue;
            _randomSeed = _seedRng.Next(int.MinValue, int.MaxValue);
        }

        public IEnumerator<int> GetEnumerator()
        {
            Random rng = new Random(_randomSeed);
            while (true)
            {
                yield return rng.Next(_fromValue, _toValue);
            }
        }
    }
}