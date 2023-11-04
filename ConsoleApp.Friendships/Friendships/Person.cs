using System;
using System.Collections.Generic;
using System.Text;
using SqlMapper.Attributes;

namespace ConsoleApp.Friendships.Friendships
{
    [Comment("Person")]
    public class Person
    {
        [PrimaryKey]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
