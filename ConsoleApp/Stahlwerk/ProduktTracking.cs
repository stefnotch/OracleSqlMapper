using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk
{
    [Comment("Wo die Produkte derzeit sind und wo sie waren")]
    public class ProduktTracking
    {
        [PrimaryKey(databaseGenerated: false)]
        public Produkt Produkt { get; set; }

        public Anlagen.Anlage.Anlagen Anlage { get; set; }

        public DateTime Zeit { get; set; }
    }
}
