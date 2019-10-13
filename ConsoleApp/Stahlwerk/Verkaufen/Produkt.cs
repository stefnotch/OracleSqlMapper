using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Verkaufen
{
    [Comment("Ein Produkt (z.B. BMW-Blech), ein Teil einer Charge")]
    public class Produkt
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public Charge Charge { get; set; }

        [Comment("Wann wurde dieses Produkt produziert")]
        public DateTime? ProduktionsZeit { get; set; }
    }
}
