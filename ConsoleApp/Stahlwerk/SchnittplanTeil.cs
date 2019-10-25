using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk
{

    // TODO: Alternatively, you could define the Schnittplan once and store the actual length in a Produkt-Tracking-Table!
    [Comment("Ein Teil eines Schnittplan, gibt an wie viele Stueck mit welcher Laenge produziert werden")]
    public class SchnittplanTeil
    {
        [PrimaryKey]
        public int Id { get; set; }

        public Charge Charge { get; set; }

        [Comment("Geplante Anzahl")]
        public int PlanAnzahl { get; set; }

        [Comment("Um wie viel die vorhergesagte/eigentliche Anzahl abweicht")]
        public int AnzahlAbweichung { get; set; }

        [Comment("Geplante Laenge")]
        public float PlanLaenge { get; set; }

        [Comment("Um wie viel die vorhergesagte/eigentliche Laenge abweicht")]
        public float LaengeAbweichung { get; set; }
    }
}
