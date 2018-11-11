// This is the neuron. The base unit of calculation

namespace NeuralNetwork
{
    using System;
    using System.Collections.Generic;

    public class Neuron 
    {
        public Neuron(
            int threshold,
            int decayCycles,
            int refractoryPeriod)
        {
            if (threshold < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), "must be greater than or equal to 0");
            }

            if (decayCycles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(decayCycles), "must be greater than 0");
            }

            if (refractoryPeriod < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(refractoryPeriod), "must be greater than or equal to 0");
            }

            InputAccumulator = 0;
            Threshold = threshold;
            DecayCycles = decayCycles;
            RefractoryPeriod = refractoryPeriod;
            OutputSignal = false;
            RefractoryCyclesLeft = 0;

            OutputNeurons = new List<Neuron>();
            InvertedOutputNeurons = new List<Neuron>();
        }

        public void InputTrigger(bool inverted = false)
        {
            if (inverted)
            {
                // Don't go negative. Just seems bad.
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
            Neuron neuron,
            bool inverted)
        {
            if (neuron == null)
            {
                throw new ArgumentNullException(nameof(neuron));
            }

            if (inverted)
            {
                InvertedOutputNeurons.Add(neuron);
            }
            else
            {
                OutputNeurons.Add(neuron);
            }
        }

        public void RemoveNeuron(Neuron neuron)
        {
            throw new NotImplementedException();
        }

        public void Fire()
        {
            if (OutputSignal)
            {
                foreach (var neuron in OutputNeurons)
                {
                    neuron.InputTrigger(false);
                }
                foreach (var neuron in InvertedOutputNeurons)
                {
                    neuron.InputTrigger(true);
                }
            }
        }

        public bool OutputSignal { get; private set; }

        /// <summary>
        /// This accumulates signals from the input neurons
        /// </summary>
        public int InputAccumulator { get; private set; }

        /// <summary>
        /// Number of cycles left in the current refractory period
        /// </summary>
        public int RefractoryCyclesLeft { get; private set; }

        /// <summary>
        /// Once the InputAccumulator passes the Threshold, the Neuron fires
        /// </summary>
        private readonly int Threshold;

        /// <summary>
        /// The number of cycles that the neuron will remain in a non-responsive refractory
        /// period after firing.
        /// </summary>
        private readonly int RefractoryPeriod;

        /// <summary>
        /// This is the number of cycles that it takes for one input to leak away
        /// </summary>
        private readonly int DecayCycles;

        /// <summary>
        /// Collection of output neurons
        /// </summary>
        private readonly List<Neuron> OutputNeurons;

        /// <summary>
        /// Collection of out neurons with inverted signal
        /// </summary>
        private readonly List<Neuron> InvertedOutputNeurons;
    }
}
