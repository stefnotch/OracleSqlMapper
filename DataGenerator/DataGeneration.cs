using System;
using System.Collections.Generic;
using System.Text;

namespace DataGenerator
{
    public static class DataGeneration
    {
        // Primitive generators
        public static IGenerator<string> Value(string value)
        {
            return new ValueGenerator<string>(value);
        }

        public static IGenerator<int> Value(int value)
        {
            return new ValueGenerator<int>(value);
        }

        public static IGenerator<float> Value(float value)
        {
            return new ValueGenerator<float>(value);
        }

        public static IGenerator<double> Value(double value)
        {
            return new ValueGenerator<double>(value);
        }

        public static IGenerator<bool> Value(bool value)
        {
            return new ValueGenerator<bool>(value);
        }

        public static IGenerator<Enum> Value(Enum value)
        {
            return new ValueGenerator<Enum>(value);
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
        public static IGenerator<DateTime> Random(DateTime fromValue, DateTime toValue)
        {
            return new RandomDateTimeGenerator(fromValue, toValue);
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
        public static IGenerator<T> SequentialFrom<T>(params IGenerator<T>[] generators)
        {
            return new SequentialFromGenerator<T>(generators);
        }

        public static RandomFromGenerator<T> RandomFrom<T>(params IGenerator<T>[] generators)
        {
            return new RandomFromGenerator<T>(generators);
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
