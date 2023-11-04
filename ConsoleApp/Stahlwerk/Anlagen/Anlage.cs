using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Anlagen
{
    public abstract class Anlage
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Comment("Der Name")]
        public string Name { get; set; }

        [Comment("Die Beschreibung")]
        public string Text { get; set; }

        [Comment("Wie oft kann die Anlage verwendet werden.")]
        public int Lebensdauer { get; set; }

        [Comment("Wie oft die Anlage verwendet wurde.")]
        public int Lebensalter { get; set; }

        [Comment("Gewicht der Anlage in Tonnen")]
        public float Gewicht { get; set; }

        public enum Anlagen
        {
            Pfanne,
            Verteiler,
            Kokille,
            Strang
        }
    }
}
