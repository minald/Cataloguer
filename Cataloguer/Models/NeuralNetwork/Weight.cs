namespace Cataloguer.Models.NeuralNetwork
{
    public class Weight
    {
        public int Id { get; set; }

        public byte FromLayer { get; set; }

        public int FromNumber { get; set; }

        public int ToNumber { get; set; }

        public float Value { get; set; }
    }
}
