using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public class RandomFromGenerator<T> : IGenerator<T>
    {
        private readonly IReadOnlyList<T> _list;
        private readonly int[] _weights;
        private readonly int _randomSeed;
        private static readonly Random _seedRng = new Random();

        public RandomFromGenerator(IReadOnlyList<T> list, int[] weights = null)
        {
            _list = list;
            _weights = weights;
            if (weights != null && _list.Count != _weights.Length)
            {
                throw new Exception("Every list element needs a corresponding weight");
            }
            _randomSeed = _seedRng.Next(int.MinValue, int.MaxValue);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Random rng = new Random(_randomSeed);

            if (_weights == null)
            {
                while (true)
                {
                    yield return _list[rng.Next(0, _list.Count)];
                }
            }
            else
            {
                int sumOfWeights = 0;
                for (int i = 0; i < _weights.Length; i++)
                {
                    sumOfWeights += _weights[i];
                }

                while (true)
                {
                    yield return BiasedRandom(rng, _weights, sumOfWeights);
                }
            }
        }

        private T BiasedRandom(Random rng, int[] weights, int sumOfWeights)
        {
            //https://stackoverflow.com/a/1761646/3492994

            int randomNumber = rng.Next(0, sumOfWeights);
            for (int i = 0; i < weights.Length; i++)
            {
                if (randomNumber < weights[i])
                    return _list[i];
                randomNumber -= weights[i];
            }

            throw new Exception("The code should always return before this happens.");
        }
    }
}