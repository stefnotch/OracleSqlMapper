using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Verkaufen
{
    [Comment("Eine Bestellung hat viele Produkte. (Nicht Produkt Typen)")]
    public class BestellungToProdukteMap
    {
        [PrimaryKey(databaseGenerated: false)]
        public Bestellung Bestellung { get; set; }

        [PrimaryKey(databaseGenerated: false)]
        public Produkt Produkt { get; set; }
    }
}
