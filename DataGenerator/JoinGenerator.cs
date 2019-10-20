using System.Collections.Generic;
using System.Linq;

namespace DataGenerator
{
    internal class JoinGenerator : IGenerator<string>
    {
        private readonly IGenerator[] _generators;

        public JoinGenerator(IGenerator[] generators)
        {
            _generators = generators;
        }

        public IEnumerator<string> GetEnumerator()
        {
            var enumerables = _generators.Select(g => g.GetEnumerator()).ToArray();
            while (true)
            {
                string returnValue = "";
                for (int i = 0; i < enumerables.Length; i++)
                {
                    enumerables[i].MoveNext();
                    returnValue += enumerables[i].Current;
                }

                yield return returnValue;
            }
        }
    }
}