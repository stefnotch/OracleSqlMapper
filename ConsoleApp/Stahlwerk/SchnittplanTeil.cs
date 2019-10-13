using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk
{

    // TODO: How does the Schnittplan stuff work, now that you also need: 
    //    Planned Values, Constantly Changing Predicted Values and Actual Values??
    // Defining the Schnittplan once doesn't really suffice anymore
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
