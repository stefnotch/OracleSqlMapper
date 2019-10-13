using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Verkaufen
{

    // TODO: Wie haengt das mit einer Bestellung zusammen? Werden die Sachen ruebergeschaufelt?
    [Comment("Eine Produkt-Menge in einem Warenkorb")]
    public class WarenkorbEintrag
    {
        [PrimaryKey]
        public int Id { get; set; }
        public Warenkorb Warenkorb { get; set; }
        public int Anzahl { get; set; }
        public Produkt Produkt { get; set; }
    }
}
