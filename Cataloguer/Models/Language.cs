namespace Cataloguer.Models
{
    public class Language
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        public Language(int id, string name, int value) : this(name, value) => Id = id;

        public Language(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
