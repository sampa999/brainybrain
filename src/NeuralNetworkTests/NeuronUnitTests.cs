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
            var neuron = new NeuralNetwork.Neuron(1, 0, false);
        }

        [TestMethod]
        public void CreateNeuronWithValidParameters()
        {
            var neuron = new NeuralNetwork.Neuron(1, 1, false);
            neuron.Should().NotBeNull();
        }

        [TestMethod]
        public void  SingleNeuronDoesntFireWithNoInput()
        {
            var neuron = new NeuralNetwork.Neuron(1, 9999, false);
            neuron.OutputSignal.Should().BeFalse();
        }

        [TestMethod]
        public void SingleNeuronFiresWithInput()
        {
            var neuron = new NeuralNetwork.Neuron(0, 9999, false);
            neuron.OutputSignal.Should().BeFalse();
            neuron.InputTrigger(false);
            neuron.OutputSignal.Should().BeFalse();
            neuron.ProcessInputs();
            neuron.OutputSignal.Should().BeTrue();
        }

        [TestMethod]
        public void AddOutputNeuron()
        {
            var neuron = new NeuralNetwork.Neuron(0, 9999, false);
            neuron.OutputSignal.Should().BeFalse();
            var nextNeuron = new NeuralNetwork.Neuron(0, 9999, false);
            nextNeuron.OutputSignal.Should().BeFalse();

            neuron.AddNeuron(nextNeuron);
        }
    }
}
