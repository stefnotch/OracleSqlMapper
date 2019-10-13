using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Anlagen
{
    public class KokilleFormat
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Comment("Eine Kokille kann mehrere Formate haben, da die Formate austauschbar sind")]
        public Kokille Kokille { get; set; }

        [Comment("Breite des Kokillenformat")]
        public float Breite { get; set; }

        [Comment("Hoehe des Kokillenformat")]
        public float Hoehe { get; set; }
    }
}
