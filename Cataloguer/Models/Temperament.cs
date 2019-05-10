namespace Cataloguer.Models
{
    public class Temperament
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        public Temperament() { }

        public Temperament(int id, string name, int value) : this(name, value) => Id = id;

        public Temperament(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
