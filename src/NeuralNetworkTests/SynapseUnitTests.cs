using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeuralNetworkTests
{
    [TestClass]
    public class SynapseUnitTests
    {
        [TestMethod]
        public void CreateSynapse()
        {
            var neuron1 = new NeuralNetwork.Neuron(1, 1);
            var neuron2 = new NeuralNetwork.Neuron(1, 1);
            var synapse = new NeuralNetwork.Synapse(neuron1, neuron2, false);
        }
    }
}
