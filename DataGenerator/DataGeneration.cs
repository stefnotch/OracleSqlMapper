using System;
using System.Collections.Generic;
using System.Text;

namespace DataGenerator
{
    public static class DataGeneration
    {
        // Primitive generators
        public static IGenerator<string> Value(string text)
        {
            return new ValueGenerator<string>(text);
        }

        public static IGenerator<int> Value(int number)
        {
            return new ValueGenerator<int>(number);
        }

        public static IGenerator<float> Value(float number)
        {
            return new ValueGenerator<float>(number);
        }

        public static IGenerator<double> Value(double number)
        {
            return new ValueGenerator<double>(number);
        }

        public static IGenerator<T>[] Values<T>(IReadOnlyList<T> values)
        {
            var generators = new ValueGenerator<T>[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                generators[i] = new ValueGenerator<T>(values[i]);
            }
            return generators;
        }

        public static IGenerator<T>[] Values<T>(params T[] values)
        {
            var generators = new ValueGenerator<T>[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                generators[i] = new ValueGenerator<T>(values[i]);
            }
            return generators;
        }

        public static IGenerator<int> Count(int fromValue = 0, int stepSize = 1)
        {
            return new CountGenerator(fromValue, stepSize);
        }

        public static IGenerator<int> Random(int toValue)
        {
            return new RandomIntGenerator(0, toValue);
        }
        public static IGenerator<int> Random(int fromValue, int toValue)
        {
            return new RandomIntGenerator(fromValue, toValue);
        }

        public static IGenerator<double> Random(double toValue)
        {
            return new RandomDoubleGenerator(0, toValue);
        }
        public static IGenerator<double> Random(double fromValue, double toValue)
        {
            return new RandomDoubleGenerator(fromValue, toValue);
        }

        // Intermediate Generators (Value generators that call higher order generators)
        // Just exist for convenience and the most common cases
        public static IGenerator<T> SequentialFrom<T>(IReadOnlyList<T> enumerable)
        {
            return SequentialFrom(Values(enumerable));
        }

        public static IGenerator<string> SequentialFrom(params string[] elements)
        {
            return SequentialFrom(Values(elements));
        }

        public static IGenerator<T> RandomFrom<T>(IReadOnlyList<T> list)
        {
            return RandomFrom(Values(list));
        }

        public static IGenerator<string> RandomFrom(params string[] elements)
        {
            return RandomFrom(Values(elements));
        }

        // Higher order generators
        public static IGenerator<T> SequentialFrom<T>(IGenerator<T>[] generators)
        {
            return new SequentialFromGenerator<T>(generators);
        }

        public static IGenerator<T> RandomFrom<T>(IGenerator<T>[] generators, int[] weights = null)
        {
            return new RandomFromGenerator<T>(generators, weights);
        }

        public static IGenerator<T> RepeatEach<T>(IGenerator<T> generator, int repeatEachElementCount)
        {
            return new RepeatGenerator<T>(generator, repeatEachElementCount);
        }

        public static IGenerator<string> Join(params IGenerator[] generators)
        {
            return new JoinGenerator(generators);
        }
    }
}
