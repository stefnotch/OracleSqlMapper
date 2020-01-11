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

        [Comment("Charge dieses Produktes")]
        public Charge Charge { get; set; }

        [Comment("Geplante Laenge, wird direkt im Produkt abgespeichert. Jedes Produkt weiss somit alles ueber sich selber, siehe Industie 4.0")]
        public float GeplanteLaenge { get; set; }

        [Comment("Um wie viel die vorhergesagte/eigentliche Laenge abweicht")]
        public float LaengeAbweichung { get; set; }

        [Comment("Wann wurde dieses Produkt produziert")]
        public DateTime? ProduktionsZeit { get; set; }
    }
}
