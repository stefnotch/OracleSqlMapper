using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Erzeugungsvorschriften
{
    [Comment("Fuer Matrix-Parameter, welche fuer Kuehlzohnen verwendet werden.")]
    public class EzvParamMatrix
    {
        [PrimaryKey]
        public int Id { get; set; }

        public EzvParam EzvParam { get; set; }
    }
}
