// This is the 'wire' connecting a neuron to another

namespace NeuralNetwork
{
    public class NeuronLink
    {
        public NeuronLink(
            Neuron neuron,
            bool inverted)
        {
            this.neuron = neuron;
            this.inverted = inverted;
        }

        public readonly Neuron neuron;
        public readonly bool inverted;
    }
}
