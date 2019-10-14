using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Erzeugungsvorschriften
{
    [Comment("Eine Sammlung an Erzeugungsvorschriften. z.B Die Gruppe Auto hat Erzeugungsvorschriften fur verschiedene Auto-Typen")]
    public class EzvGruppe
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
