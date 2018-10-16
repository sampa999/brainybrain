// This is the neuron. The base unit of calculation

namespace NeuralNetwork
{
    using System.Collections.Generic;

    public class Neuron
    {
        public Neuron()
        {
            OutgoingNeurons = new List<NeuronLink>();
            IncomingNeurons = new List<Neuron>();
        }

        public readonly List<NeuronLink> OutgoingNeurons;
        public readonly List<Neuron> IncomingNeurons;
    }
}
