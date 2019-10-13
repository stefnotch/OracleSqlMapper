using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Erzeugungsvorschriften
{
    // TODO: I don't actually need this, do I? I can just use EzvParamMatrixElement
    [Comment("Fuer Matrix-Parameter, welche fuer Kuehlzohnen verwendet werden.")]
    public class EzvParamMatrix
    {
        [PrimaryKey]
        public int Id { get; set; }

        public EzvParam EzvParam { get; set; }
    }
}
