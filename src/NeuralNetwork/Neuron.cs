// This is the neuron. The base unit of calculation

namespace NeuralNetwork
{
    using System;
    using System.Collections.Generic;

    public class Neuron
    {
        public Neuron(
            int threshold,
            int decayCycles)
        {
            if (decayCycles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(decayCycles), "must be greater than 0");
            }

            IncomingNeurons = new List<Neuron>();
            OutputNeurons = new List<Neuron>();
            InputAccumulator = 0;
            Threshold = threshold;
            DecayCycles = decayCycles;
        }

        public readonly List<Neuron> IncomingNeurons;

        public readonly List<Neuron> OutputNeurons;

        public void InputTrigger(int value)
        {
            InputAccumulator += value;
        }

        public void ProcessInputs()
        {

        }

        public void Fire()
        {

        }

        /// <summary>
        /// This accumulates signals from the input neurons
        /// </summary>
        private int InputAccumulator;

        /// <summary>
        /// Once the InputAccumulator passes the Threshold, the Neuron fires
        /// </summary>
        private int Threshold;

        /// <summary>
        /// This is the number of cycles that it takes for one input to leak away
        /// </summary>
        private int DecayCycles;
    }
}
