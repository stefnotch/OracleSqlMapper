using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Verkaufen
{
    [Comment("Mehrere Bestellungen werden gruppiert und auf einmal geliefert")]
    public class Lieferung
    {
        [PrimaryKey]
        public int Id { get; set; }

        public Lieferdienst Lieferdienst { get; set; }

        public string Status { get; set; }

        public DateTime LieferDatum { get; set; }
    }
}
