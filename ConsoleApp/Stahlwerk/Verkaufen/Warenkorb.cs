using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Verkaufen
{
    [Comment("Ein Warenkorb/Kunde")]
    public class Warenkorb
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
