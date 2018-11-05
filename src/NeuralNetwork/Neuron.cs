// This is the neuron. The base unit of calculation

namespace NeuralNetwork
{
    using System;
    using System.Collections.Generic;

    public class Neuron : IInputNeuron, IOutputNeuron
    {
        public Neuron(
            int threshold,
            int decayCycles,
            bool inverted)
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

            OutputNeurons = new List<IInputNeuron>();
        }

        public void InputTrigger(bool inverted)
        {
            if (inverted)
            {
                if (InputAccumulator > 0)
                {
                    InputAccumulator--;
                }
            }
            else
            {
                InputAccumulator++;
            }
        }

        public void ProcessInputs()
        {
            OutputSignal = InputAccumulator > Threshold;
            if (OutputSignal)
            {
                InputAccumulator = 0;
            }
        }

        public void AddNeuron(
            IInputNeuron neuron)
        {
            if (neuron == null)
            {
                throw new ArgumentNullException(nameof(neuron));
            }

            OutputNeurons.Add(neuron);
        }

        public void RemoveNeuron(IInputNeuron neuron)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Collection of output neurons
        /// </summary>
        private readonly List<IInputNeuron> OutputNeurons;

        /// <summary>
        /// True if output is inverted
        /// </summary>
        private bool Inverted;
    }
}
