using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    /// <summary>
    /// Interface for neuron that can produce an output
    /// </summary>
    public interface IOutputNeuron
    {
        /// <summary>
        /// Add a neuron that will be triggered when we produce an output
        /// </summary>
        /// 
        void AddNeuron(IInputNeuron neuron);

        /// <summary>
        /// Removes a neuron that was previously added. If it doesn't exist, it just returns.
        /// </summary>
        /// <param name="neuron"></param>
        void RemoveNeuron(IInputNeuron neuron);
    }
}
