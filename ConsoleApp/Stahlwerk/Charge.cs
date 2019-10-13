﻿using System;
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

        // TODO: Don't we need an associative table or something here? 
        // TODO: Previously I just had Ezv here, which can't be right, can it?
        public EzvGruppe EzvGruppe { get; set; }

        [Comment("Mit welchem Format wird diese Charge gemacht")]
        public KokilleFormat KokilleFormat { get; set; }
    }
}
