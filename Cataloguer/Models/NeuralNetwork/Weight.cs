namespace Cataloguer.Models.NeuralNetwork
{
    public class Weight
    {
        public int Id { get; set; }

        public byte FromLayer { get; set; }

        public int FromNumber { get; set; }

        public int ToNumber { get; set; }

        public float Value { get; set; }

        public Weight() { }

        public Weight(byte fromLayer, int fromNumber, int toNumber, float value)
        {
            FromLayer = fromLayer;
            FromNumber = fromNumber;
            ToNumber = toNumber;
            Value = value;
        }
    }
}
