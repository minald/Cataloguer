namespace Cataloguer.Models.NeuralNetwork
{
    public class Bias
    {
        public int Id { get; set; }

        public byte Layer { get; set; } // 0 - input, 1 - hidden, 2 - output

        public int Number { get; set; }

        public float Value { get; set; }

        public Bias() { }

        public Bias(byte layer, int number, float value)
        {
            Layer = layer;
            Number = number;
            Value = value;
        }
    }
}
