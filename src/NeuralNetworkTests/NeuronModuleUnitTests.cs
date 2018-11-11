using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;
using FluentAssertions;

namespace NeuralNetworkTests
{
    /// <summary>
    /// Summary description for NeuronModuleUnitTests
    /// </summary>
    [TestClass]
    public class NeuronModuleUnitTests
    {
        public NeuronModuleUnitTests()
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
        public void ConstructNeuronModule()
        {
            var neuronModule = new NeuronModule();
            neuronModule.Should().NotBeNull();
            neuronModule.neurons.Should().BeEmpty();
        }

        [TestMethod]
        public void RunEmptyNeuronModule()
        {
            var neuronModule = new NeuronModule();
            neuronModule.Process();
            neuronModule.Fire();
        }

        [TestMethod]
        public void RunOneElementNeuronModule()
        {
            var neuronModule = new NeuronModule();
            var neuron = new Neuron(0, 99999, 0);
            neuronModule.neurons.Add(neuron);
            neuron.OutputSignal.Should().BeFalse();
            neuron.InputTrigger();
            neuronModule.Run();
            neuron.OutputSignal.Should().BeTrue();
        }
    }
}
