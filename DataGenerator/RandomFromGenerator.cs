using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public class RandomFromGenerator<T> : IGenerator<T>
    {
        private readonly IReadOnlyList<T> _list;
        private static readonly Random _rng = new Random();

        public RandomFromGenerator(IReadOnlyList<T> list)
        {
            _list = list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (true)
            {
                yield return _list[_rng.Next(0, _list.Count)];
            }
        }
    }
}