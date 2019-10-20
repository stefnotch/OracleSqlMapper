using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public class RandomFromGenerator<T> : IGenerator<T>
    {
        private readonly IReadOnlyList<T> _list;
        private readonly int _randomSeed;
        private static readonly Random _seedRng = new Random();

        public RandomFromGenerator(IReadOnlyList<T> list)
        {
            _list = list;
            _randomSeed = _seedRng.Next(int.MinValue, int.MaxValue);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Random rng = new Random(_randomSeed);
            while (true)
            {
                yield return _list[rng.Next(0, _list.Count)];
            }
        }
    }
}