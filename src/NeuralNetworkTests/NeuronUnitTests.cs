using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeuralNetworkTests
{
    [TestClass]
    public class NeuronUnitTests
    {
        [TestMethod]
        public void CreateNeuron()
        {
            var neuron = new NeuralNetwork.Neuron(0, 0);

        }
    }
}
