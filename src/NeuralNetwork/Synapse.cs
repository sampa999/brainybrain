using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Synapse
    {
        public Synapse(
            Neuron input, 
            Neuron output,
            bool invert)
        {
            Input = input;
            Output = output;
            Invert = invert;
        }

        public readonly Neuron Input;
        public readonly Neuron Output;
        public bool Invert;
    }
}
