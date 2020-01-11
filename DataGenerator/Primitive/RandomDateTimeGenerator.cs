using System;
using System.Collections.Generic;
using System.Text;

namespace DataGenerator
{
    public class RandomDateTimeGenerator : IGenerator<DateTime>
    {
        private readonly DateTime _fromValue;
        private readonly DateTime _toValue;
        private readonly int _randomSeed;
        private static readonly Random _seedRng = new Random();
        public RandomDateTimeGenerator(DateTime fromValue, DateTime toValue)
        {
            _fromValue = fromValue;
            _toValue = toValue;
            _randomSeed = _seedRng.Next(int.MinValue, int.MaxValue);
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            Random rng = new Random(_randomSeed);
            while (true)
            {
                yield return _fromValue + new TimeSpan((long)(rng.NextDouble() * (_toValue - _fromValue).Ticks));
            }
        }
    }
}
