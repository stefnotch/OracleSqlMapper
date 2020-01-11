using System;
using System.Collections.Generic;
using System.Text;
using DataGenerator;

namespace CSharpToSqlMapper
{
    internal class CSharpToSqlGenerator : IGenerator<string>
    {
        private readonly IGenerator _generator;
        private readonly TypeMapping _typeMapping;

        internal CSharpToSqlGenerator(IGenerator generator, TypeMapping typeMapping)
        {
            _generator = generator;
            _typeMapping = typeMapping;
        }

        public IEnumerator<string> GetEnumerator()
        {
            var enumerable = _generator.GetEnumerator();
            while (true)
            {
                enumerable.MoveNext();
                yield return _typeMapping.ValueToSql(enumerable.Current);
            }
        }
    }
}
