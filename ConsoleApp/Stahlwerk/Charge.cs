using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp.Stahlwerk.Anlagen;
using ConsoleApp.Stahlwerk.Erzeugungsvorschriften;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk
{
    [Comment("Eine Charge, also eine Menge an Stahl die verarbeitet wird.")]
    public class Charge
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        [Comment("Geplantes Gewicht")]
        public float PlanGewicht { get; set; }

        [Comment("Um wie viel das vorhergesagte/eigentliche Gewicht abweicht")]
        public float GewichtAbweichung { get; set; }

        [Comment("Die Ezv der Charge, z.B. Auto-Audi")]
        public Ezv Ezv { get; set; }

        [Comment("Mit welchem Format wird diese Charge gemacht")]
        public KokilleFormat KokilleFormat { get; set; }
    }

    /* TODO: A ChargeLog Table which tells you where a charge currently is might be useful
     * KokilleTimestamp
     * PfanneTimestamp
     * StrangTimest...
     * 
     */
}
