using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Anlagen
{
    [Comment("Welche Chargen sind gerade in dieser Anlage")]
    public abstract class ChargeToAnlageMap<T> where T : Anlage
    {
        [PrimaryKey]
        public T Anlage { get; set; }

        [PrimaryKey]
        public Charge Charge { get; set; }

        public DateTime StartZeit { get; set; }
    }
}
