﻿using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk
{
    [Comment("Ein Produkt Typ.")]
    public class ProduktTyp
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public float GeplanteLaenge { get; set; }
        [Comment("Wie viel dieses Produkt fuer einen Kunden kostet in USD")]
        public float Preis { get; set; }
    }
}
