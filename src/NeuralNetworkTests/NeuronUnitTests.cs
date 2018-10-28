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

        [TestMethod]
        public void  SingleNeuronDoesntFireWithNoInput()
        {
            var neuron = new NeuralNetwork.Neuron(1, 9999);
            neuron.OutputSignal.Should().BeFalse();
        }

        [TestMethod]
        public void SingleNeuronFiresWithInput()
        {
            var neuron = new NeuralNetwork.Neuron(0, 9999);
            neuron.OutputSignal.Should().BeFalse();
            neuron.InputTrigger();
            neuron.OutputSignal.Should().BeFalse();
            neuron.ProcessInputs();
            neuron.OutputSignal.Should().BeTrue();
        }

    }
}
