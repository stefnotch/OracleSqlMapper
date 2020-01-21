using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp.Stahlwerk.Erzeugungsvorschriften;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk
{
    [Comment("Ein Parameter eines Produktes, wird fuer die Qualitaetsparameter des Produktes verwendent")]
    public class ProduktParam
    {
        [PrimaryKey]
        public int Id { get; set; }

        public Produkt Produkt { get; set; }

        public EzvParam EzvParam { get; set; }

        public int Wert { get; set; }
    }
}
