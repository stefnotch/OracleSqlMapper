using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Erzeugungsvorschriften
{
    [Comment("Fuer Matrix-Parameter, welche fuer Kuehlzohnen verwendet werden.")]
    public class EzvParamMatrixElement
    {
        [PrimaryKey]
        public int Id { get; set; }
        public EzvParam EzvParam { get; set; }

        [Comment("Der Index von diesem Element in der Matrix")]
        public int MatrixIndex { get; set; }

        [Comment("Der Wert von diesem Element")]
        public float Wert { get; set; }
    }
}
