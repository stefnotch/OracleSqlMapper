using System;

namespace SqlMapper.SqlObjects
{
    public abstract class Constraint : DatabaseObject
    {
        public Table Table { get; }

        /// <summary>
        /// If this constraint is defined inline (create table) or not (alter table)
        /// </summary>
        public bool IsInline { get; set; }

        /// <summary>
        /// If this constraint only affects a single column, this will be set
        /// </summary>
        public abstract TableColumn SingleAffectedColumn { get; }

        /// <summary>
        /// If this constraint should be defined inline (create table) and doesn't need a column list
        /// </summary>
        protected bool IsInlineSingleColumn => IsInline && SingleAffectedColumn != null;

        protected Constraint(Table table, string name) : base(name)
        {
            Table = table;
            Schema = table.Schema;

            /* Suggested names
            * references (foreign key)	fk
               unique	un
               primary key	pk
               check	ck
               not null	nn
               index	idx*/
        }

        public override string ToString()
        {
            return $"CONSTRAINT {SqlName} ";
        }
    }
}