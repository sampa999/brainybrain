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
            neuron.InputTrigger(false);
            neuron.OutputSignal.Should().BeFalse();
            neuron.ProcessInputs();
            neuron.OutputSignal.Should().BeTrue();
        }

        [TestMethod]
        public void AddOutputNeuron()
        {
            var neuron = new NeuralNetwork.Neuron(0, 9999);
            neuron.OutputSignal.Should().BeFalse();
            var nextNeuron = new NeuralNetwork.Neuron(0, 9999);
            nextNeuron.OutputSignal.Should().BeFalse();

            neuron.AddNeuron(nextNeuron, false);
        }

        /// <summary>
        /// N1 -> N2
        /// Input to N1 should make N2 fire
        /// </summary>
        [TestMethod]
        public void TwoNeuronChain()
        {
            var neuron = new NeuralNetwork.Neuron(0, 9999);
            neuron.OutputSignal.Should().BeFalse();
            var nextNeuron = new NeuralNetwork.Neuron(0, 9999);
            nextNeuron.OutputSignal.Should().BeFalse();

            neuron.AddNeuron(nextNeuron, false);

            neuron.InputTrigger(false);
            neuron.ProcessInputs();
            neuron.OutputSignal.Should().BeTrue();
            nextNeuron.ProcessInputs();
            nextNeuron.OutputSignal.Should().BeFalse();
            neuron.Fire();
            nextNeuron.Fire();

            neuron.ProcessInputs();
            neuron.OutputSignal.Should().BeFalse();
            nextNeuron.ProcessInputs();
            nextNeuron.OutputSignal.Should().BeTrue();
        }
    }
}
