using SqlMapper.Attributes;

namespace ConsoleApp.Stahlwerk.Verkaufen
{
    public class Lieferdienst
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}