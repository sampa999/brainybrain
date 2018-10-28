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

            if (threshold < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), "must be greater than or equal to 0");
            }

            InputAccumulator = 0;
            Threshold = threshold;
            DecayCycles = decayCycles;
            OutputSignal = false;
        }

        public void InputTrigger()
        {
            InputAccumulator++;
        }

        public void ProcessInputs()
        {
            OutputSignal = InputAccumulator > Threshold;
            InputAccumulator = 0;
        }

        public bool OutputSignal { get; private set; }

        /// <summary>
        /// This accumulates signals from the input neurons
        /// </summary>
        public int InputAccumulator { get; private set; }

        /// <summary>
        /// Once the InputAccumulator passes the Threshold, the Neuron fires
        /// </summary>
        private readonly int Threshold;

        /// <summary>
        /// This is the number of cycles that it takes for one input to leak away
        /// </summary>
        private readonly int DecayCycles;
    }
}
