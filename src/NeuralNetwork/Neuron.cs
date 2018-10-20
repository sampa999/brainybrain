// This is the neuron. The base unit of calculation

namespace NeuralNetwork
{
    using System.Collections.Generic;

    public class Neuron
    {
        public Neuron()
        {
            IncomingNeurons = new List<Neuron>();
            InputAccumulator = 0;
        }

        public readonly List<Neuron> IncomingNeurons;
        public void InputTrigger(int value)
        {
            InputAccumulator += value;
        }

        public void Fire()
        {

        }

        private int InputAccumulator;
    }
}
