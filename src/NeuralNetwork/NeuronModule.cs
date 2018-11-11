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
            neuronCollections = new List<NeuronModule>();
        }

        public void Process()
        {
            foreach (var neuronCollection in neuronCollections)
            {
                neuronCollection.Process();
            }

            foreach (var neuron in neurons)
            {
                neuron.ProcessInputs();
            }
        }

        public void Fire()
        {
            foreach (var neuronCollection in neuronCollections)
            {
                neuronCollection.Fire();
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
        public readonly List<NeuronModule> neuronCollections;
    }
}
