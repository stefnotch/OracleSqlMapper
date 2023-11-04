using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DataGenerator;
using SqlMapper;
using SqlMapper.Attributes;
using SqlMapper.SqlObjects;
using SqlMapper.SqlObjects.Constraints;
using SqlMapper.SqlPrimitives;

namespace CSharpToSqlMapper
{
    public class Mapper
    {
        private readonly Dictionary<Type, TableMapping> _tableMappings = new Dictionary<Type, TableMapping>();
        private readonly Dictionary<Type, TypeMapping> _typeMappings = new Dictionary<Type, TypeMapping>();

        public Schema Schema { get; } = new Schema();

        public void Initialize()
        {
            Schema.Initialize();
        }

        private TypeMapping GetOrAddTypeMapping(Type type, int? size = null)
        {
            if (!_typeMappings.TryGetValue(type, out var typeMapping))
            {
                typeMapping = TypeMapping.From(type, size);
                _typeMappings.Add(type, typeMapping);
            }
            return typeMapping;
        }

        private TypeMapping GetTypeMapping(Type type)
        {
            return _typeMappings[type];
        }

        public void AddTable<T>(string tag = null) where T : class
        {
            if (string.IsNullOrEmpty(tag))
            {
                tag = default;
            }



            // Create a table with our Name: typeof(T).Name
            var table = new Table(typeof(T).Name) { Tag = tag };




            var tableMapping = new TableMapping(typeof(T), table);
            _tableMappings.Add(typeof(T), tableMapping);
            Schema.Add(table);
            foreach (var commentAttribute in typeof(T).GetCustomAttributes<CommentAttribute>(true))
            {
                table.Comment += commentAttribute.Comment + "\n";
            }




            // Get all properties --> Table Columns
            // TODO: GetMembers, not only the properties
            var propertyInfos = typeof(T)
               .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
               .ToList();





            var primaryKey = new PrimaryKeyConstraint(table, "pk_" + table.SqlName, Enumerable.Empty<TableColumn>())
            {
                Tag = tag,
                IsInline = true
            };




            foreach (var prop in propertyInfos)
            {
                // Reading attributes from our properties
                int? size = prop.GetCustomAttribute<MaxLengthAttribute>()?.Length;



                // Etc.


                var typeMapping = GetOrAddTypeMapping(prop.PropertyType, size);

                bool isForeignKey = typeMapping.SqlType.IsReference;
                var column = new TableColumn(table, (isForeignKey ? "id_" : "") + prop.Name, typeMapping.SqlType);
                table.Columns.Add(column);
                tableMapping.Columns.Add(prop.Name, column);

                bool isNullableType = (!prop.PropertyType.IsValueType) || (Nullable.GetUnderlyingType(prop.PropertyType) != null);
                if (!isNullableType)
                {
                    table.Constraints.Add(new NotNullConstraint(table, $"nn_{table.SqlName}_{column.SqlName}", column) { Tag = tag });
                }

                var primaryKeyAttribute = prop.GetCustomAttribute<PrimaryKeyAttribute>(true);
                if (primaryKeyAttribute != null)
                {
                    primaryKey.Columns.Add(column);
                    if (primaryKeyAttribute.DatabaseGenerated)
                    {
                        tableMapping.DatabaseGenerated = true;
                        var sequence = new Sequence($"seq_{table.SqlName}") { Tag = tag };
                        tableMapping.PrimaryKeySequence = sequence;
                        Schema.Add(sequence);
                        var trigger = new Trigger($"trigger_{table.SqlName}")
                        {
                            Tag = tag,
                            SqlCode = $"    BEFORE INSERT ON {table.SqlName}\n" +
                                      $"    FOR EACH ROW\n" +
                                      $"BEGIN\n" +
                                      $"    IF :NEW.{column.SqlName} IS NULL THEN\n" +
                                      $"        SELECT {sequence.SqlName}.NEXTVAL INTO :NEW.{column.SqlName} FROM SYS.DUAL;\n" +
                                      $"    END IF;\n" +
                                      $"END;"
                        };
                        Schema.Add(trigger);
                    }
                }

                // TODO: Check, Unique, Default Value
                if (prop.GetCustomAttribute<UniqueAttribute>(true) != null)
                {
                    table.Constraints.Add(new UniqueConstraint(table, $"uq_{table.SqlName}_{column.SqlName}", column) { Tag = tag });
                }

                column.Comment += prop.GetCustomAttributes<CommentAttribute>(true)
                    .Select(prop => prop.Comment)
                    .ToDelimitedString("\n");

                if (typeMapping is EnumTypeMapping enumTypeMapping)
                {
                    var checkInValues = enumTypeMapping.EnumSqlNames
                        .Select(name => $"'{name}'")
                        .ToDelimitedString(", ");

                    table.Constraints.Add(new CheckConstraint(table, $"ck_{table.SqlName}_{column.SqlName}", $"{column.SqlName} IN ({checkInValues})") { Tag = tag });
                }

                if (isForeignKey)
                {
                    Schema.OnInitialise += () =>
                    {
                        // Initialize the foreign key constraints
                        var referencedColumn = _tableMappings[prop.PropertyType].Table
                        .Constraints
                        .OfType<PrimaryKeyConstraint>()
                        .Single()
                        .Columns
                        .Single();
                        table.Constraints.Add(new ForeignKeyConstraint(table, $"fk_{table.SqlName}_{column.SqlName}", column, referencedColumn) { Tag = tag });
                    };
                }
            }


            if (primaryKey.Columns.Count > 0)
            {
                table.Constraints.Add(primaryKey);
            }

        }

        public InsertStatementGenerator<T> InsertsFor<T>(int count, string tag) where T : class
        {
            return new InsertStatementGenerator<T>(this, count, tag);
        }

        public string ToStringCreateOrReplace(string tag)
        {
            return Schema.ToStringDropWithTag(tag) + Schema.ToStringCreateWithTag(tag);
        }

        public string ToStringAlter(string tag)
        {
            return Schema.ToStringAlterWithTag(tag);
        }
        public string ToStringTriggersAndSequences(string tag)
        {
            return Schema.ToStringSequencesWithTag(tag) + "\n" + Schema.ToStringTriggersWithTag(tag);
        }

        public string ToStringInserts(string tag)
        {
            return Schema.ToStringInsertsWithTag(tag);
        }

        public override string ToString()
        {
            return Schema.ToString();
        }

        public class InsertStatementGenerator<T> : IReadOnlyList<int> where T : class
        {
            private readonly Mapper _mapper;
            private readonly TableMapping _tableMapping;
            private readonly GeneratorInsertAction _insertStatements;
            private readonly IGenerator<int> _primaryKeyGenerator;

            public int Count => _insertStatements.InsertCount;

            public int this[int index]
            {
                get
                {
                    // For Foreign Keys
                    if (_primaryKeyGenerator is IIndexedGenerator<int> indexedGenerator)
                    {
                        return indexedGenerator[index];
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            public InsertStatementGenerator(Mapper mapper, int count, string tag)
            {
                _mapper = mapper;
                _tableMapping = _mapper._tableMappings[typeof(T)];
                _insertStatements = new GeneratorInsertAction(_tableMapping.Table)
                {
                    Tag = tag,
                    Schema = mapper.Schema
                };

                if (_tableMapping.DatabaseGenerated)
                {
                    var primaryKeyColumn = _tableMapping.Table
                                            .Constraints
                                            .OfType<PrimaryKeyConstraint>()
                                            .Single()
                                            .SingleAffectedColumn;

                    int startValue = _tableMapping.PrimaryKeySequence.StartWith ?? 0;
                    _primaryKeyGenerator = DataGeneration.Count(startValue);
                    _tableMapping.PrimaryKeySequence.StartWith = startValue + count;
                    _insertStatements.Generators.Add(primaryKeyColumn, _primaryKeyGenerator);
                }

                _insertStatements.Generate(count);
            }

            public InsertStatementGenerator<T> Set<TValue>(Expression<Func<T, TValue>> getExpression, IGenerator generator)
            {
                string memberName = GetMemberName(getExpression);
                var typeMapping = _mapper.GetTypeMapping(typeof(TValue));
                _insertStatements.Generators.Add(_tableMapping.Columns[memberName], new CSharpToSqlGenerator(generator, typeMapping));
                return this;
            }

            private static string GetMemberName<TDelegate>(Expression<TDelegate> expression)
            {
                // https://stackoverflow.com/a/52305334/3492994
                return expression.Body switch
                {
                    MemberExpression m => m.Member.Name,
                    UnaryExpression u when u.Operand is MemberExpression m => m.Member.Name,
                    _ => throw new NotImplementedException(expression.GetType().ToString()),
                };
            }

            public IEnumerator<int> GetEnumerator()
            {
                var primaryKeys = _primaryKeyGenerator.GetEnumerator();
                int count = Count;
                for (int i = 0; i < count; i++)
                {
                    primaryKeys.MoveNext();
                    yield return primaryKeys.Current;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
