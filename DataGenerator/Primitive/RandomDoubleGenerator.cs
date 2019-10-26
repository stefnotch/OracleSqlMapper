using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public class RandomDoubleGenerator : IGenerator<double>
    {
        private readonly double _fromValue;
        private readonly double _toValue;
        private readonly int _randomSeed;
        private static readonly Random _seedRng = new Random();
        public RandomDoubleGenerator(double fromValue, double toValue)
        {
            _fromValue = fromValue;
            _toValue = toValue;
            _randomSeed = _seedRng.Next(int.MinValue, int.MaxValue);
        }

        public IEnumerator<double> GetEnumerator()
        {
            Random rng = new Random(_randomSeed);
            while (true)
            {
                yield return rng.NextDouble() * (_toValue - _fromValue) + _fromValue;
            }
        }
    }
}