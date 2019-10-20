using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public class RandomDoubleGenerator : IGenerator<double>
    {
        private readonly double _fromValue;
        private readonly double _toValue;
        private static readonly Random _rng = new Random();
        public RandomDoubleGenerator(double fromValue, double toValue)
        {
            _fromValue = fromValue;
            _toValue = toValue;
        }

        public IEnumerator<double> GetEnumerator()
        {
            while (true)
            {
                yield return _rng.NextDouble() * (_toValue - _fromValue) + _fromValue;
            }
        }
    }
}