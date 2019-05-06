namespace Cataloguer.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        public Country() { }

        public Country(int id, string name, int value) : this(name, value) => Id = id;

        public Country(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
