using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlMapper.SqlObjects;

namespace SqlMapper
{
    /// <summary>
    /// A schema. Equivalent to a user.
    /// </summary>
    public class Schema
    {
        private readonly List<DatabaseObject> _databaseObjects = new List<DatabaseObject>();
        private readonly List<DatabaseAction> _databaseActions = new List<DatabaseAction>();
        public string Name { get; set; }
        public IReadOnlyCollection<DatabaseObject> DatabaseObjects => _databaseObjects;
        public IReadOnlyCollection<DatabaseAction> DatabaseActions => _databaseActions;

        public event Action OnInitialise;

        public void Initialize()
        {
            OnInitialise?.Invoke();
        }

        public void Add(DatabaseObject databaseObject)
        {
            databaseObject.Schema = this;
        }

        internal void AddInternal(DatabaseObject databaseObject)
        {
            string sqlName = SqlUtils.ToSqlName(databaseObject.Name);
            int nameLength = sqlName.Length;
            int counter = 0;
            while (DatabaseObjects.Any(dbo => dbo.SqlName == sqlName))
            {
                string counterText = counter + "";
                sqlName = sqlName.Substring(0, nameLength - counterText.Length) + counterText;
                counter++;
            }
            databaseObject.SqlName = sqlName;


            _databaseObjects.Add(databaseObject);
        }

        public void Add(DatabaseAction databaseAction)
        {
            databaseAction.Schema = this;
        }

        internal void AddInternal(DatabaseAction databaseAction)
        {
            _databaseActions.Add(databaseAction);
        }


        public void Remove(DatabaseObject databaseObject)
        {
            databaseObject.Schema = null;
        }

        internal void RemoveInternal(DatabaseObject databaseObject)
        {
            _databaseObjects.Remove(databaseObject);
        }

        public void Remove(DatabaseAction databaseAction)
        {
            databaseAction.Schema = null;
        }

        internal void RemoveInternal(DatabaseAction databaseAction)
        {
            _databaseActions.Remove(databaseAction);
        }


        public string ToStringDropWithTag(string tag)
        {
            return ToStringObjectsWithTag<Table>(tag, ToStringDrop);
        }

        public string ToStringCreateWithTag(string tag)
        {
            return ToStringObjectsWithTag<Table>(tag, ToStringCreate);
        }

        public string ToStringAlterWithTag(string tag)
        {
            return ToStringObjectsWithTag<Table>(tag, ToStringAlter);
        }

        public string ToStringSequencesWithTag(string tag)
        {
            return ToStringObjectsWithTag<Sequence>(tag, ToStringSequences);
        }

        public string ToStringTriggersWithTag(string tag)
        {
            return ToStringObjectsWithTag<Trigger>(tag, ToStringTriggers);
        }

        public string ToStringInsertsWithTag(string tag)
        {
            return ToStringActionsWithTag<InsertAction>(tag, ToStringInserts);
        }

        private string ToStringObjectsWithTag<T>(string tag, Action<StringBuilder, List<T>> toStringObjects) where T : DatabaseObject
        {
            StringBuilder sqlCode = new StringBuilder();
            var databaseObjects = DatabaseObjects.OfType<T>().Where(t => t.Tag == tag).ToList();
            if (databaseObjects.Count == 0) return "";
            toStringObjects(sqlCode, databaseObjects);
            return sqlCode.ToString();
        }

        private string ToStringActionsWithTag<T>(string tag, Action<StringBuilder, List<T>> toStringActions) where T : DatabaseAction
        {
            StringBuilder sqlCode = new StringBuilder();
            var databaseActionos = DatabaseActions.OfType<T>().Where(t => t.Tag == tag).ToList();
            if (databaseActionos.Count == 0) return "";
            toStringActions(sqlCode, databaseActionos);
            return sqlCode.ToString();
        }


        public override string ToString()
        {
            StringBuilder sqlCode = new StringBuilder();
            var tables = DatabaseObjects.OfType<Table>().ToList();
            if (tables.Count == 0) return "";
            ToStringDrop(sqlCode, tables);
            ToStringCreate(sqlCode, tables);
            ToStringAlter(sqlCode, tables);
            ToStringSequences(sqlCode, DatabaseObjects.OfType<Sequence>().ToList());
            ToStringTriggers(sqlCode, DatabaseObjects.OfType<Trigger>().ToList());
            return sqlCode.ToString();
        }

        private static void ToStringDrop(StringBuilder sqlCode, List<Table> tables)
        {
            sqlCode.Append(@"DECLARE
    PROCEDURE SAFE_DROP_TABLE (p_table_name varchar2) as
    BEGIN
       EXECUTE IMMEDIATE 'DROP TABLE ' || p_table_name || ' CASCADE CONSTRAINTS';
       DBMS_OUTPUT.PUT_LINE('Table ' || p_table_name || ' dropped.');
    EXCEPTION
       WHEN OTHERS THEN
          IF SQLCODE != -942 THEN
             RAISE;
          ELSE
             DBMS_OUTPUT.PUT_LINE('Table ' || p_table_name || ' skipped.');
          END IF;
    END;
BEGIN
");
            foreach (var table in tables)
            {
                sqlCode.Append($"    SAFE_DROP_TABLE('{table.SqlName}');");
                sqlCode.Append("\n");
            }
            sqlCode.Append("END;\n/\n\n");
        }

        private static void ToStringCreate(StringBuilder sqlCode, List<Table> tables)
        {
            foreach (var table in tables)
            {
                sqlCode.Append(table.ToStringCreate(false));
                sqlCode.Append("\n\n");
            }
        }

        private static void ToStringAlter(StringBuilder sqlCode, List<Table> tables)
        {
            foreach (var table in tables)
            {
                sqlCode.Append(table.ToStringAlter());
                sqlCode.Append("\n");
            }
        }

        private static void ToStringSequences(StringBuilder sqlCode, List<Sequence> sequences)
        {
            sqlCode.Append(@"DECLARE
    PROCEDURE SAFE_DROP_SEQ (p_seq_name varchar2) as
    BEGIN
       EXECUTE IMMEDIATE 'DROP SEQUENCE ' || p_seq_name;
       DBMS_OUTPUT.PUT_LINE('Sequence ' || p_seq_name || ' dropped.');
    EXCEPTION
       WHEN OTHERS THEN
          IF SQLCODE != -2289 THEN
             RAISE;
          ELSE
             DBMS_OUTPUT.PUT_LINE('Sequence ' || p_seq_name || ' skipped.');
          END IF;
    END;
BEGIN
");

            foreach (var sequence in sequences)
            {
                sqlCode.Append($"    SAFE_DROP_SEQ('{sequence.SqlName}');");
                sqlCode.Append("\n");
            }
            sqlCode.Append("END;\n/\n\n");

            foreach (var sequence in sequences)
            {
                sqlCode.Append(sequence.ToStringCreate(false));
                sqlCode.Append("\n");
            }
        }

        private static void ToStringTriggers(StringBuilder sqlCode, List<Trigger> triggers)
        {
            foreach (var trigger in triggers)
            {
                sqlCode.Append(trigger.ToStringCreate(true));
                sqlCode.Append("\n");
            }
        }

        private static void ToStringInserts(StringBuilder sqlCode, List<InsertAction> inserts)
        {
            bool useBlockInsert = inserts.Sum(i => i.InsertCount) > 15;
            if (useBlockInsert)
            {
                sqlCode.Append("BEGIN\n");
            }
            foreach (var insert in inserts)
            {
                sqlCode.Append(insert.ToStringExecute());
                sqlCode.Append("\n");
            }
            if (useBlockInsert)
            {
                sqlCode.Append("EXCEPTION\n");
                sqlCode.Append("    WHEN OTHERS THEN\n");
                sqlCode.Append("        DBMS_OUTPUT.PUT_LINE(SQLCODE || SQLERRM);\n");
                sqlCode.Append("        RAISE;\n");
                sqlCode.Append("END;\n");
                sqlCode.Append("/\n");
            }
        }
    }
}
