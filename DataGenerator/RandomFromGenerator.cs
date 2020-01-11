using System;
using System.Collections.Generic;
using System.Linq;

namespace DataGenerator
{
    public class RandomFromGenerator<T> : IGenerator<T>
    {
        private readonly IGenerator<T>[] _generators;
        private readonly int[] _weights;
        private readonly int _sumOfWeights;
        private readonly int _randomSeed;
        private static readonly Random _seedRng = new Random();

        public RandomFromGenerator(IGenerator<T>[] generators, int[] weights = null)
        {
            _generators = generators;
            if (weights == null)
            {
                weights = new int[_generators.Length];
                for (int i = 0; i < weights.Length; i++)
                {
                    weights[i] = 1;
                }
            }
            _weights = weights;
            if (_generators.Length != _weights.Length)
            {
                throw new Exception("Every generator needs a corresponding weight");
            }
            _sumOfWeights = 0;
            for (int i = 0; i < _weights.Length; i++)
            {
                _sumOfWeights += _weights[i];
            }

            _randomSeed = _seedRng.Next(int.MinValue, int.MaxValue);
        }

        public RandomFromGenerator<T> WithWeights(params int[] weights)
        {
            return new RandomFromGenerator<T>(_generators, weights);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Random rng = new Random(_randomSeed);
            var enumerators = _generators.Select(g => g.GetEnumerator()).ToList();

            if (_weights == null)
            {
                while (true)
                {
                    int index = rng.Next(0, _generators.Length);
                    enumerators[index].MoveNext();
                    yield return enumerators[index].Current;
                }
            }
            else
            {
                while (true)
                {
                    int index = BiasedRandom(rng, _weights, _sumOfWeights);
                    enumerators[index].MoveNext();
                    yield return enumerators[index].Current;
                }
            }
        }

        private int BiasedRandom(Random rng, int[] weights, int sumOfWeights)
        {
            //https://stackoverflow.com/a/1761646/3492994

            int randomNumber = rng.Next(0, sumOfWeights);
            for (int i = 0; i < weights.Length; i++)
            {
                if (randomNumber < weights[i])
                    return i;
                randomNumber -= weights[i];
            }

            throw new Exception("The code should always return before this happens.");
        }
    }
}