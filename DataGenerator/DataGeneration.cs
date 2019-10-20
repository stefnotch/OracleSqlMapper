using System;
using System.Collections.Generic;
using System.Text;

namespace DataGenerator
{
    public static class DataGeneration
    {
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

        public static IGenerator<T> RandomFrom<T>(IReadOnlyList<T> list)
        {
            return new RandomFromGenerator<T>(list);
        }

        public static IGenerator<string> RandomFrom(params string[] elements)
        {
            return new RandomFromGenerator<string>(elements);
        }

        public static IGenerator<T> SequentialFrom<T>(IEnumerable<T> enumerable)
        {
            return new SequentialFromGenerator<T>(enumerable);
        }

        public static IGenerator<string> SequentialFrom(params string[] elements)
        {
            return new SequentialFromGenerator<string>(elements);
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
