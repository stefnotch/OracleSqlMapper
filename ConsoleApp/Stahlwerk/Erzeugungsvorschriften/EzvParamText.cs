using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Erzeugungsvorschriften
{
    [Comment("Fuer Text-Parameter. Jeder Parmeter-Wert wird einem Text zugeordnet.")]
    public class EzvParamText
    {
        [PrimaryKey]
        public int Id { get; set; }

        public EzvParam EzvParam { get; set; }

        [Comment("Der Wert der diesem Text zugeordnet ist.")]
        public int TextWert { get; set; }

        [Comment("Der Text fuer den Wert")]
        public string Text { get; set; }
    }
}
