using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Erzeugungsvorschriften
{
    [Comment("Assoziative Tabelle zwischen Erzeugungsvorschriften und deren Parameter.")]
    public class EzvToParamMap
    {
        [PrimaryKey(databaseGenerated: false)]
        public Ezv Ezv { get; set; }

        [PrimaryKey(databaseGenerated: false)]
        public EzvParam EzvParam { get; set; }

        [Comment("Obergrenze, also ein nicht idealer, aber akzeptabler Wert")]
        public float MaxWert { get; set; }
        [Comment("Ziel Wert, also der ideale Wert")]
        public float ZielWert { get; set; }
        [Comment("Untergrenze, also ein nicht idealer, aber akzeptabler Wert")]
        public float MinWert { get; set; }
    }
}
