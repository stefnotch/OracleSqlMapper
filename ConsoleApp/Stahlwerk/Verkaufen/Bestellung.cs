using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Verkaufen
{
    [Comment("Eine Bestellung, Teil einer Lieferung")]
    public class Bestellung
    {
        [PrimaryKey]
        public int Id { get; set; }
        public Warenkorb Warenkorb { get; set; }
        public int Anzahl { get; set; }
        public Produkt Produkt { get; set; }
        public Lieferung Lieferung { get; set; }
    }
}
