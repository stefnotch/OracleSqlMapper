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

namespace ConsoleApp
{
    public class CSharpToSqlMapper
    {
        protected class TableMapping
        {
            public TableMapping(Type type, Table table)
            {
                Type = type;
                Table = table;
            }

            public Type Type { get; }
            public Table Table { get; }
            public Dictionary<string, TableColumn> Columns { get; } = new Dictionary<string, TableColumn>();
            public List<object> InsertStatementGenerators { get; } = new List<object>();
        }


        private readonly Dictionary<Type, TableMapping> _tableMappings = new Dictionary<Type, TableMapping>();

        public readonly Schema Schema = new Schema();

        public void Initialise()
        {
            Schema.Initialise();
        }

        public void AddTable(Type type, string tag = null)
        {
            if (string.IsNullOrEmpty(tag))
            {
                tag = default;
            }

            var table = new Table(type.Name) { Tag = tag };
            var tableMapping = new TableMapping(type, table);
            _tableMappings.Add(type, tableMapping);
            Schema.Add(table);

            foreach (var commentAttribute in type.GetCustomAttributes<CommentAttribute>(true))
            {
                table.Comment += commentAttribute.Comment + "\n";
            }

            // TODO: GetMembers, not only the properties
            var propertyInfos = type
               .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
               .ToList();

            var primaryKey = new PrimaryKeyConstraint(table, "pk_" + table.SqlName, Enumerable.Empty<TableColumn>())
            {
                Tag = tag,
                IsInline = true
            };
            foreach (var prop in propertyInfos)
            {
                var datatype = Datatype.FromPropertyInfo(prop, out bool isForeignKey);
                var column = new TableColumn(table, (isForeignKey ? "id_" : "") + prop.Name, datatype);
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
                        var sequence = new Sequence($"seq_{table.SqlName}") { Tag = tag };
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

                if (prop.PropertyType.IsEnum)
                {
                    var enumValueNames = prop.PropertyType.GetFields()
                        .Where(fi => fi.IsStatic)
                        .OrderBy(fi => fi.Name)
                        .Select(fi => fi.Name.Trim())
                        .ToList();

                    var enumSqlNames = enumValueNames
                        .Select(name => SqlUtils.PascalCaseToSnakeCase(name))
                        .Select(name =>
                        {
                            string[] nameParts = name.Split("_");
                            if (nameParts.Length <= 1)
                            {
                                return nameParts[0].Substring(0, 4);
                            }
                            else if (nameParts.Length == 2)
                            {
                                return $"{nameParts[0].Substring(0, 2)}{nameParts[1].Substring(0, 2)}";
                            }
                            else if (nameParts.Length == 3)
                            {
                                return $"{nameParts[0].Substring(0, 2)}{nameParts[1].Substring(0, 1)}{nameParts[2].Substring(0, 1)}";
                            }
                            else
                            {
                                return $"{nameParts[0][0]}{nameParts[1][0]}{nameParts[2][0]}{nameParts[3][0]}";
                            }
                        })
                        .ToList();

                    for (int i = 0; i < enumSqlNames.Count; i++)
                    {
                        int nameLength = enumSqlNames[i].Length;
                        int counter = 0;
                        while (enumSqlNames.Take(i).Contains(enumSqlNames[i]))
                        {
                            string counterText = counter + "";
                            enumSqlNames[i] = enumSqlNames[i].Substring(0, nameLength - counterText.Length) + counterText;
                            counter++;
                        }
                    }

                    var checkInValues = enumSqlNames
                        .Select(name => $"'{name}'")
                        .ToDelimitedString(", ");

                    column.Comment += enumSqlNames
                        .Select((sqlName, index) => $"{sqlName}: {enumValueNames[index]}")
                        .ToDelimitedString("\n");
                    table.Constraints.Add(new CheckConstraint(table, $"ck_{table.SqlName}_{column.SqlName}", $"{column.SqlName} IN ({checkInValues})") { Tag = tag });
                }

                if (isForeignKey)
                {
                    //prop.PropertyType
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

        public InsertStatementGenerator<T> InsertsFor<T>() where T : class
        {
            return new InsertStatementGenerator<T>(this);
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
            private readonly CSharpToSqlMapper _mapper;
            private readonly TableMapping _tableMapping;
            private readonly Lazy<IGenerator<int>> _primaryKeyGenerator;

            public int Count => _tableMapping.Table.InsertsCount;

            public int this[int index]
            {
                get
                {
                    // For Foreign Keys
                    if (_primaryKeyGenerator.Value is IIndexedGenerator<int> indexedGenerator)
                    {
                        return indexedGenerator[index];
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            public InsertStatementGenerator(CSharpToSqlMapper mapper)
            {
                _mapper = mapper;
                _tableMapping = _mapper._tableMappings[typeof(T)];
                _tableMapping.InsertStatementGenerators.Add(this);

                _primaryKeyGenerator = new Lazy<IGenerator<int>>(() =>
                {
                    var generator = _tableMapping.Table
                                        .Constraints
                                        .OfType<PrimaryKeyConstraint>()
                                        .Single()
                                        .SingleAffectedColumn
                                        .Generator;
                    return (IGenerator<int>)generator;
                });
            }

            public InsertStatementGenerator<T> Set<TValue>(Expression<Func<T, TValue>> getExpression, IGenerator generator)
            {
                string memberName = GetMemberName(getExpression);
                _tableMapping.Columns[memberName].Generator = generator;
                return this;
            }

            public InsertStatementGenerator<T> Generate(int count)
            {
                _tableMapping.Table.InsertsCount = count;
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
                var primaryKeys = _primaryKeyGenerator.Value.GetEnumerator();
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
