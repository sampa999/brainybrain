using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace NeuralNetworkTests
{
    [TestClass]
    public class NeuronUnitTests
    {
        public NeuronUnitTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateNeuronWithInvalidThreshold()
        {
            var neuron = new NeuralNetwork.Neuron(-1, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateNeuronWithInvalidDecayCycles()
        {
            var neuron = new NeuralNetwork.Neuron(1, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateNeuronWithInvalidRefractoryPeriod()
        {
            var neuron = new NeuralNetwork.Neuron(1, 1, -1);
        }

        [TestMethod]
        public void CreateNeuronWithValidParameters()
        {
            var neuron = new NeuralNetwork.Neuron(1, 1, 0);
            neuron.Should().NotBeNull();
        }

        [TestMethod]
        public void  SingleNeuronDoesntFireWithNoInput()
        {
            var neuron = new NeuralNetwork.Neuron(1, 9999, 0);
            neuron.OutputSignal.Should().BeFalse();
        }

        [TestMethod]
        public void SingleNeuronFiresWithInput()
        {
            var neuron = new NeuralNetwork.Neuron(0, 9999, 0);
            neuron.OutputSignal.Should().BeFalse();
            neuron.InputTrigger(false);
            neuron.OutputSignal.Should().BeFalse();
            neuron.ProcessInputs();
            neuron.OutputSignal.Should().BeTrue();
        }

        [TestMethod]
        public void AddOutputNeuron()
        {
            var neuron = new NeuralNetwork.Neuron(0, 9999, 0);
            neuron.OutputSignal.Should().BeFalse();
            var nextNeuron = new NeuralNetwork.Neuron(0, 9999, 0);
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
            var neuron = new NeuralNetwork.Neuron(0, 9999, 0);
            neuron.OutputSignal.Should().BeFalse();
            var nextNeuron = new NeuralNetwork.Neuron(0, 9999, 0);
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
