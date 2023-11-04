using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Friendships.Friendships
{
    [Comment("Friendship between two persons. Could also be used for followers")]
    public class Friendship
    {
        [Comment("From Person")]
        public Person PersonA { get; set; }

        [Comment("To Person")]
        public Person PersonB { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
