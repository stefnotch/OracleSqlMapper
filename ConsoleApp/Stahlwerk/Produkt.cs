using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk
{
    [Comment("Ein Produkt (z.B. BMW-Blech), ein Teil einer Charge")]
    public class Produkt
    {
        [PrimaryKey]
        public int Id { get; set; }
        public ProduktTyp ProduktTyp { get; set; }

        [Comment("Index dieses Produktes in der Charge")]
        public int IndexInCharge { get; set; }

        [Comment("Charge dieses Produktes")]
        public Charge Charge { get; set; }

        [Comment("Um wie viel die vorhergesagte/eigentliche Laenge abweicht")]
        public float LaengeAbweichung { get; set; }

        [Comment("Wann wurde dieses Produkt produziert")]
        public DateTime? ProduktionsZeit { get; set; }
    }
}
