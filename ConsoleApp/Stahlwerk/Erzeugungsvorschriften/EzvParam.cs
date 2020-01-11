using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Erzeugungsvorschriften
{
    [Comment("Ein Parameter fuer Erzeugungsvorschriften. Er kann mehreren Vorschriften zugewiesen werden.")]
    public class EzvParam
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        [Comment("Physikalisch groesstmoeglicher Wert")]
        public float MaxWert { get; set; }

        [Comment("Der Wert am Anfang")]
        public float StandardWert { get; set; }

        [Comment("Physikalisch kleinstmoeglicher Wert")]
        public float MinWert { get; set; }

        [Comment("Einheit des Wert. z.B. kg")]
        public string Einheit { get; set; }

        [Comment("Die Quelle der Messung. z.B. Automatisch Elektronisch, Externe Datenbank, Manuelle Eingabe")]
        public string MessQuelle { get; set; }

        [Comment("Ob die Messung automatisch erfolgt")]
        public bool MessAutomatisch { get; set; }

        [Comment("Der Bereich wo es gemessen wurde. z.B. Kokille")]
        public string MessBereich { get; set; }

        [Comment("Der Unterbereich wo es gemessen wurde")]
        public string MessUnterBereich { get; set; }

        [Comment("Ob dieser Parameter derzeit aktiv ist. Damit kann man einen nicht verwendeten Parameter ausschalten ohne ihn zu loeschen.")]
        public bool IstAktiv { get; set; }

        public enum DatenTyp
        {
            Text,
            Matrix
        }

        public DatenTyp? ParamDatenTyp { get; set; }

        public enum EzvParamTyp
        {
            Produktionsparameter,
            Qualitaetsparameter
        }

        public EzvParamTyp ParamTyp { get; set; }
    }
}
