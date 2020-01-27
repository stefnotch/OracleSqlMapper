using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk
{
    [Comment("Ein Parameter fuer ein Segment des Produktes. z.B. Ein Blech hat mehrere Segmente mit verschiedenen Qualitaeten")]
    public class ProduktSegmentParam
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Comment("Index von diesem Segment")]
        public int Segment { get; set; }

        public ProduktParam ProduktParam { get; set; }

        public int Wert { get; set; }
    }
}
