using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Verkaufen
{

    [Comment("Eine Produkt-Menge in einem Warenkorb. Wird in die Bestellung uebertragen")]
    public class WarenkorbEintrag
    {
        [PrimaryKey]
        public int Id { get; set; }
        public Warenkorb Warenkorb { get; set; }
        public int Anzahl { get; set; }
        public ProduktTyp ProduktTyp { get; set; }
    }
}
