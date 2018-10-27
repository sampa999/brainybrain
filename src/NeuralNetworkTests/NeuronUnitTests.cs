using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace NeuralNetworkTests
{
    [TestClass]
    public class NeuronUnitTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateNeuronWithInvalidDecayCycles()
        {
            var neuron = new NeuralNetwork.Neuron(1, 0);

        }

        [TestMethod]
        public void CreateNeuronWithValidParameters()
        {
            var neuron = new NeuralNetwork.Neuron(1, 1);
            neuron.Should().NotBeNull();
        }
    }
}
