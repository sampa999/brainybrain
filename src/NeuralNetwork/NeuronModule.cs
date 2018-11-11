using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuronModule
    {
        public NeuronModule()
        {
            neurons = new List<Neuron>();
            neuronModules = new List<NeuronModule>();
        }

        public void Process()
        {
            foreach (var neuronModule in neuronModules)
            {
                neuronModule.Process();
            }

            foreach (var neuron in neurons)
            {
                neuron.ProcessInputs();
            }
        }

        public void Fire()
        {
            foreach (var neuronModule in neuronModules)
            {
                neuronModule.Fire();
            }

            foreach (var neuron in neurons)
            {
                neuron.Fire();
            }
        }

        public void Run()
        {
            this.Process();
            this.Fire();
        }

        public readonly List<Neuron> neurons;
        public readonly List<NeuronModule> neuronModules;
    }
}
