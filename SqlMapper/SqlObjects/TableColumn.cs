using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator;
using SqlMapper.SqlPrimitives;

namespace SqlMapper.SqlObjects
{
    public class TableColumn : DatabaseObjectVariable<Table>
    {
        public TableColumn(Table databaseObject, string name, Datatype datatype) : base(databaseObject, name, datatype)
        {
        }
    }
}
