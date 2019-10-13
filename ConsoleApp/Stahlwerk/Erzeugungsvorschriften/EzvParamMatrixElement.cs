using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Erzeugungsvorschriften
{
    [Comment("Ein einzelnes Element eines Ezv Matrix-Parameter")]
    public class EzvParamMatrixElement
    {
        [PrimaryKey]
        public int Id { get; set; }
        public EzvParamMatrix EzvParamMatrix { get; set; }

        [Comment("Der Index von diesem Element in der Matrix")]
        public int MatrixIndex { get; set; }

        [Comment("Der Wert von diesem Element")]
        public float Wert { get; set; }
    }
}
