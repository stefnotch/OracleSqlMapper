using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Erzeugungsvorschriften
{
    [Comment("Erzeugungsvorschrift\nEine Vorschrift wie etwas zu produzieren ist.\nHat eine Menge an Parameter die bei der Produktion relevant sind.")]
    public class Ezv
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public EzvGruppe EzvGruppe { get; set; }
    }
}
