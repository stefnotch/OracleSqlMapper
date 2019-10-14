using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SqlMapper;
using SqlMapper.Attributes;
using SqlMapper.SqlObjects;
using SqlMapper.SqlObjects.Constraints;
using SqlMapper.SqlPrimitives;

namespace ConsoleApp
{
    public class CSharpToSqlMapper
    {
        private readonly Dictionary<Type, Table> _tables = new Dictionary<Type, Table>();
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
            _tables.Add(type, table);
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
                var column = new TableColumn(table, (isForeignKey ? "id_" : "") + SqlUtils.ToSqlName(prop.Name), datatype);
                table.Columns.Add(column);

                bool isNullableType = (!prop.PropertyType.IsValueType) || (Nullable.GetUnderlyingType(prop.PropertyType) != null);
                if (!isNullableType)
                {
                    table.Constraints.Add(new NotNullConstraint(table, $"nn_{table.SqlName}_{column.Name}", column) { Tag = tag });
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
                                      $"    IF :NEW.{column.Name} IS NULL THEN\n" +
                                      $"        SELECT {sequence.SqlName}.NEXTVAL INTO :NEW.{column.Name} FROM SYS.DUAL;\n" +
                                      $"    END IF;\n" +
                                      $"END;"
                        };
                        Schema.Add(trigger);
                    }
                }

                // TODO: Check, Unique, Default Value
                if (prop.GetCustomAttribute<UniqueAttribute>(true) != null)
                {
                    table.Constraints.Add(new UniqueConstraint(table, $"uq_{table.SqlName}_{column.Name}", column) { Tag = tag });
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
                    table.Constraints.Add(new CheckConstraint(table, $"ck_{table.SqlName}_{column.Name}", $"{column.Name} IN ({checkInValues})") { Tag = tag });
                }

                if (isForeignKey)
                {
                    //prop.PropertyType
                    Schema.OnInitialise += () =>
                    {
                        // Initialize the foreign key constraints
                        var referencedColumn = _tables[prop.PropertyType]
                        .Constraints
                        .OfType<PrimaryKeyConstraint>()
                        .Single()
                        .Columns
                        .Single();
                        table.Constraints.Add(new ForeignKeyConstraint(table, $"fk_{table.SqlName}_{column.Name}", column, referencedColumn) { Tag = tag });
                    };
                }
            }


            if (primaryKey.Columns.Count > 0)
            {
                table.Constraints.Add(primaryKey);
            }

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

        public override string ToString()
        {
            return Schema.ToString();
        }
    }
}
