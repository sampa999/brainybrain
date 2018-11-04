using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    /// <summary>
    /// Interface for a Neuron that can take an input
    /// </summary>
    public interface IInputNeuron
    {
        void InputTrigger();
    }
}
