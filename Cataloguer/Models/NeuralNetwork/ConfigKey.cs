namespace Cataloguer.Models.NeuralNetwork
{
    public class ConfigKey
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public ConfigKey() { }

        public ConfigKey(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
