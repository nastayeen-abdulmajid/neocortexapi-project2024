// Copyright (c) Damir Dobric. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoCortex;
using NeoCortexApi;
using NeoCortexApi.Entities;
using NeoCortexApi.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace UnitTestsProject
{
    [TestClass]
    public class AdaptSynapses
    {
        int inputBits = 88;
        int numColumns = 1024;
        private Parameters parameters;
        private SpatialPooler SpatialPooler;
        private Connections htmConnections;

        /// <summary>
        /// Sets up default parameters for the Hierarchical Temporal Memory (HTM) configuration.
        /// </summary>
        /// <returns>An instance of <see cref="HtmConfig"/> with default parameters.</returns>
        private HtmConfig SetupHtmConfigDefaultParameters()
        {
            // Create a new instance of HtmConfig with specified input and column dimensions
            var htmConfig = new HtmConfig(new int[] { 32, 32 }, new int[] { 64, 64 })
            {
                PotentialRadius = 16,
                PotentialPct = 0.5,
                GlobalInhibition = false,
                LocalAreaDensity = -1.0,
                NumActiveColumnsPerInhArea = 10.0,
                StimulusThreshold = 0.0,
                SynPermInactiveDec = 0.008,
                SynPermActiveInc = 0.05,
                SynPermConnected = 0.10,
                MinPctOverlapDutyCycles = 0.001,
                MinPctActiveDutyCycles = 0.001,
                DutyCyclePeriod = 1000,
                MaxBoost = 10.0,
                RandomGenSeed = 42,

                // Initialize the Random property with a ThreadSafeRandom instance using a seed value
                Random = new ThreadSafeRandom(42)
            };

            return htmConfig;
        }

        /// <summary>
        /// Unit test method for the 'AdaptSynapses' function with maximum threshold value.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void TestAdaptSynapsesWithMaxThreshold()
        {
            // Initialization with default parameters from HtmConfig
            var htmConfig = SetupHtmConfigDefaultParameters();
            htmConfig.InputDimensions = new int[] { 8 };
            htmConfig.ColumnDimensions = new int[] { 4 };
            htmConfig.SynPermInactiveDec = 0.01;
            htmConfig.SynPermActiveInc = 0.1;
            htmConfig.WrapAround = false;

            // Creating Connections and SpatialPooler instances
            htmConnections = new Connections(htmConfig);
            SpatialPooler = new SpatialPooler();
            SpatialPooler.Init(htmConnections);

            // Setting the maximum threshold value for synaptic permanence trimming
            htmConnections.HtmConfig.SynPermTrimThreshold = .05;

            // Defining potential pools for columns
            int[][] potentialPools = new int[][] {
            new int[]{ 1, 1, 1, 1, 0, 0, 0, 0 },
            new int[]{ 1, 0, 0, 0, 1, 1, 0, 1 },
            new int[]{ 0, 0, 1, 0, 0, 0, 1, 0 },
            new int[]{ 1, 0, 0, 0, 0, 0, 1, 0 }
            };

            // Initializing permanences for each column
            double[][] permanences = new double[][] {
            new double[]{ 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450 },
            new double[]{ 0.000, 0.000, 0.014, 0.000, 0.000, 0.000, 0.110, 0.000 },
            new double[]{ 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            // Expected true permanences after adaptation
            double[][] truePermanences = new double[][] {
            new double[]{ 0.300, 0.110, 0.080, 0.140, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.250, 0.000, 0.000, 0.000, 0.280, 0.110, 0.000, 0.440 },
            new double[]{ 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.210, 0.000 },
            new double[]{ 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            // Setting up potential pools and permanences for each column in Connections
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(potentialPools[i], (n) => (n == 1));
                htmConnections.GetColumn(i).SetProximalConnectedSynapsesForTest(htmConnections, indexes);
                htmConnections.GetColumn(i).SetPermanences(htmConnections.HtmConfig, permanences[i]);
            }

            // Input vector and active columns for testing
            int[] inputVector = new int[] { 1, 0, 0, 1, 1, 0, 1, 0 };
            int[] activeColumns = new int[] { 0, 1, 2 };

            // Executing the AdaptSynapses method with the specified parameters 
            SpatialPooler.AdaptSynapses(htmConnections, inputVector, activeColumns);

            // Asserting that the adapted permanences match the expected true permanences
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                double[] perms = htmConnections.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(htmConnections.HtmConfig.NumInputs);
                for (int j = 0; j < truePermanences[i].Length; j++)
                {
                    Assert.IsTrue(Math.Abs(truePermanences[i][j] - perms[j]) <= 0.01);
                }
            }
        }

        /// <summary>
        /// Unit test for the 'AdaptSynapses' method with a single set of permanences.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void TestAdaptSynapsesWithSinglePermanences()
        {
            // Initialization with HtmConfig
            var htmConfig = SetupHtmConfigDefaultParameters();
            htmConfig.InputDimensions = new int[] { 8 };
            htmConfig.ColumnDimensions = new int[] { 4 };
            htmConfig.SynPermInactiveDec = 0.01;
            htmConfig.SynPermActiveInc = 0.1;
            htmConfig.WrapAround = false;
            htmConnections = new Connections(htmConfig);
            SpatialPooler = new SpatialPooler();
            SpatialPooler.Init(htmConnections);

            // Set synapse trim threshold
            htmConnections.HtmConfig.SynPermTrimThreshold = 0.05;

            // Define potential pools for columns
            int[][] potentialPools = new int[][] {
            new int[]{ 1, 1, 1, 1, 0, 0, 0, 0 }
            };

            // Initialize permanences
            double[][] permanences = new double[][] {
            new double[]{ 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000 }
            };

            // Define expected true permanences after adaptation
            double[][] truePermanences = new double[][] {
            new double[]{ 0.300, 0.110, 0.080, 0.140, 0.000, 0.000, 0.000, 0.000 }
            };

            // Set proximal connected synapses and initial permanences for each column
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns - 4; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(potentialPools[i], (n) => (n == 1));
                htmConnections.GetColumn(i).SetProximalConnectedSynapsesForTest(htmConnections, indexes);
                htmConnections.GetColumn(i).SetPermanences(htmConnections.HtmConfig, permanences[i]);
            }

            // Set input vector and active columns
            int[] inputVector = new int[] { 1, 0, 0, 1, 1, 0, 1, 0 };
            int[] activeColumns = new int[] { 0 };

            // Execute the AdaptSynapses method with parameters 
            SpatialPooler.AdaptSynapses(htmConnections, inputVector, activeColumns);

            // Validate that the actual permanences match the expected true permanences
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns - 4; i++)
            {
                double[] perms = htmConnections.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(htmConnections.HtmConfig.NumInputs);
                for (int j = 0; j < truePermanences[i].Length; j++)
                {
                    Assert.IsTrue(Math.Abs(truePermanences[i][j] - perms[j]) <= 0.01);
                }
            }
        }


        /// <summary>
        /// Unit test method to verify the adaptation of synapses with two permanences.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void TestAdaptSynapsesWithTwoPermanences()
        {
            // Initialize Hyperparameter Configuration
            var htmConfig = SetupHtmConfigDefaultParameters();
            htmConfig.InputDimensions = new int[] { 8 };
            htmConfig.ColumnDimensions = new int[] { 4 };
            htmConfig.SynPermInactiveDec = 0.01;
            htmConfig.SynPermActiveInc = 0.1;
            htmConfig.WrapAround = false;

            // Create HTM Connections and Spatial Pooler instances
            htmConnections = new Connections(htmConfig);
            SpatialPooler = new SpatialPooler();
            SpatialPooler.Init(htmConnections);

            // Set the Synapse Trim Threshold
            htmConnections.HtmConfig.SynPermTrimThreshold = 0.05;

            // Define potential pools for columns
            int[][] potentialPools = new int[][] {
            new int[]{ 1, 1, 1, 1, 0, 0, 0, 0 },
            new int[]{ 1, 0, 0, 0, 1, 1, 0, 1 }
            };

            // Initialize synapse permanences
            double[][] permanences = new double[][] {
            new double[]{ 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450 }
            };

            // Define expected true permanences after adaptation
            double[][] truePermanences = new double[][] {
            new double[]{ 0.300, 0.110, 0.080, 0.140, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.250, 0.000, 0.000, 0.000, 0.280, 0.110, 0.000, 0.440 }
            };

            // Set up proximal synapses for each column
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns - 4; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(potentialPools[i], (n) => (n == 1));
                htmConnections.GetColumn(i).SetProximalConnectedSynapsesForTest(htmConnections, indexes);
                htmConnections.GetColumn(i).SetPermanences(htmConnections.HtmConfig, permanences[i]);
            }

            // Define input vector and active columns
            int[] inputVector = new int[] { 1, 0, 0, 1, 1, 0, 1, 0 };
            int[] activeColumns = new int[] { 0, 1 };

            // Execute the AdaptSynapses method with specified parameters 
            SpatialPooler.AdaptSynapses(htmConnections, inputVector, activeColumns);

            // Verify that the adapted permanences match the expected true permanences
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns - 4; i++)
            {
                double[] perms = htmConnections.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(htmConnections.HtmConfig.NumInputs);
                for (int j = 0; j < truePermanences[i].Length; j++)
                {
                    Assert.IsTrue(Math.Abs(truePermanences[i][j] - perms[j]) <= 0.01);
                }
            }
        }


        /// <summary>
        /// Unit test method for the 'AdaptSynapses' function with a minimum threshold value.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void TestAdaptSynapsesWithMinThreshold()
        {
            // Initialization with default HtmConfig parameters.
            var htmConfig = SetupHtmConfigDefaultParameters();
            htmConfig.InputDimensions = new int[] { 8 };
            htmConfig.ColumnDimensions = new int[] { 4 };
            htmConfig.SynPermInactiveDec = 0.01;
            htmConfig.SynPermActiveInc = 0.1;
            htmConfig.WrapAround = false;

            // Creating Connections object and initializing SpatialPooler.
            htmConnections = new Connections(htmConfig);
            SpatialPooler = new SpatialPooler();
            SpatialPooler.Init(htmConnections);

            // Setting the minimum threshold value for synaptic permanences trimming.
            htmConnections.HtmConfig.SynPermTrimThreshold = 0.01;

            // Defining potential pools, initialized permanences, and true permanences.
            int[][] potentialPools = new int[][]
            {
            new int[] { 1, 1, 1, 1, 0, 0, 0, 0 },
            new int[] { 1, 0, 0, 0, 1, 1, 0, 1 },
            new int[] { 0, 0, 1, 0, 0, 0, 1, 0 },
            new int[] { 1, 0, 0, 0, 0, 0, 1, 0 }
            };

            double[][] permanences = new double[][]
            {
            new double[] { 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000 },
            new double[] { 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450 },
            new double[] { 0.000, 0.000, 0.014, 0.000, 0.000, 0.000, 0.110, 0.000 },
            new double[] { 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            double[][] truePermanences = new double[][]
            {
            new double[] { 0.300, 0.110, 0.080, 0.140, 0.000, 0.000, 0.000, 0.000 },
            new double[] { 0.250, 0.000, 0.000, 0.000, 0.280, 0.110, 0.000, 0.440 },
            new double[] { 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.210, 0.000 },
            new double[] { 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            // Setting up proximal connected synapses and initial permanences for each column.
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(potentialPools[i], (n) => (n == 1));
                htmConnections.GetColumn(i).SetProximalConnectedSynapsesForTest(htmConnections, indexes);
                htmConnections.GetColumn(i).SetPermanences(htmConnections.HtmConfig, permanences[i]);
            }

            // Defining input vector and active columns.
            int[] inputVector = new int[] { 1, 0, 0, 1, 1, 0, 1, 0 };
            int[] activeColumns = new int[] { 0, 1, 2 };

            // Executing the AdaptSynapses method with the specified parameters.
            SpatialPooler.AdaptSynapses(htmConnections, inputVector, activeColumns);

            // Verifying that the resulting permanences match the expected true permanences.
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                double[] perms = htmConnections.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(htmConnections.HtmConfig.NumInputs);
                for (int j = 0; j < truePermanences[i].Length; j++)
                {
                    Assert.IsTrue(Math.Abs(truePermanences[i][j] - perms[j]) <= 0.01);
                }
            }
        }


        /// <summary>
        /// Unit test method to validate the adaptation of synapses with three permanences.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void TestAdaptSynapsesWithThreePermanences()
        {
            // Initialization with default HTM configuration parameters
            var htmConfig = SetupHtmConfigDefaultParameters();
            htmConfig.InputDimensions = new int[] { 8 };
            htmConfig.ColumnDimensions = new int[] { 4 };
            htmConfig.SynPermInactiveDec = 0.01;
            htmConfig.SynPermActiveInc = 0.1;
            htmConfig.WrapAround = false;

            // Creating HTM connections and spatial pooler instances
            htmConnections = new Connections(htmConfig);
            SpatialPooler = new SpatialPooler();
            SpatialPooler.Init(htmConnections);

            // Setting specific threshold value for synapse trimming
            htmConnections.HtmConfig.SynPermTrimThreshold = 0.05;

            // Defining potential pools for each column
            int[][] potentialPools = new int[][] {
            new int[]{ 1, 1, 1, 1, 0, 0, 0, 0 },
            new int[]{ 1, 0, 0, 0, 1, 1, 0, 1 },
            new int[]{ 0, 0, 1, 0, 0, 0, 1, 0 }
            };

            // Initializing permanences for each synapse
            double[][] permanences = new double[][] {
            new double[]{ 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450 },
            new double[]{ 0.000, 0.000, 0.014, 0.000, 0.000, 0.000, 0.110, 0.000 }
            };

            // Expected true permanences after synapse adaptation
            double[][] truePermanences = new double[][] {
            new double[]{ 0.300, 0.110, 0.080, 0.140, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.250, 0.000, 0.000, 0.000, 0.280, 0.110, 0.000, 0.440 },
            new double[]{ 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.210, 0.000 }
            };

            // Setting up connected synapses and initial permanences for each column
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns - 4; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(potentialPools[i], (n) => (n == 1));
                htmConnections.GetColumn(i).SetProximalConnectedSynapsesForTest(htmConnections, indexes);
                htmConnections.GetColumn(i).SetPermanences(htmConnections.HtmConfig, permanences[i]);
            }

            // Simulating input vector and active columns
            int[] inputVector = new int[] { 1, 0, 0, 1, 1, 0, 1, 0 };
            int[] activeColumns = new int[] { 0, 1, 2 };

            // Executing the AdaptSynapses method with specified parameters 
            SpatialPooler.AdaptSynapses(htmConnections, inputVector, activeColumns);

            // Verifying the adapted permanences match the expected true permanences
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns - 4; i++)
            {
                double[] perms = htmConnections.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(htmConnections.HtmConfig.NumInputs);
                for (int j = 0; j < truePermanences[i].Length; j++)
                {
                    Assert.IsTrue(Math.Abs(truePermanences[i][j] - perms[j]) <= 0.01);
                }
            }
        }


        /// <summary>
        /// Unit test for the "AdaptSynapses" method with four permanences.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void TestAdaptSynapsesWithFourPermanences()
        {
            // Initialization with HtmConfig parameters.
            var htmConfig = SetupHtmConfigDefaultParameters();
            htmConfig.InputDimensions = new int[] { 8 };
            htmConfig.ColumnDimensions = new int[] { 4 };
            htmConfig.SynPermInactiveDec = 0.01;
            htmConfig.SynPermActiveInc = 0.1;
            htmConfig.WrapAround = false;

            // Create Connections and SpatialPooler instances.
            htmConnections = new Connections(htmConfig);
            SpatialPooler = new SpatialPooler();
            SpatialPooler.Init(htmConnections);

            // Set SynPermTrimThreshold value.
            htmConnections.HtmConfig.SynPermTrimThreshold = .05;

            // Define potential pools for each column.
            int[][] potentialPools = new int[][] {
            new int[]{ 1, 1, 1, 1, 0, 0, 0, 0 },
            new int[]{ 1, 0, 0, 0, 1, 1, 0, 1 },
            new int[]{ 0, 0, 1, 0, 0, 0, 1, 0 },
            new int[]{ 1, 0, 0, 0, 0, 0, 1, 0 }
            };

            // Initialize permanences for each column.
            double[][] permanences = new double[][] {
            new double[]{ 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450 },
            new double[]{ 0.000, 0.000, 0.014, 0.000, 0.000, 0.000, 0.110, 0.000 },
            new double[]{ 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            // Define true permanences after adaptation.
            double[][] truePermanences = new double[][] {
            new double[]{ 0.300, 0.110, 0.080, 0.140, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.250, 0.000, 0.000, 0.000, 0.280, 0.110, 0.000, 0.440 },
            new double[]{ 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.210, 0.000 },
            new double[]{ 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            // Set proximal connected synapses and initial permanences for each column.
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(potentialPools[i], (n) => (n == 1));
                htmConnections.GetColumn(i).SetProximalConnectedSynapsesForTest(htmConnections, indexes);
                htmConnections.GetColumn(i).SetPermanences(htmConnections.HtmConfig, permanences[i]);
            }

            // Set input vector and active columns.
            int[] inputVector = new int[] { 1, 0, 0, 1, 1, 0, 1, 0 };
            int[] activeColumns = new int[] { 0, 1, 2 };

            // Execute the AdaptSynapses method with parameters.
            SpatialPooler.AdaptSynapses(htmConnections, inputVector, activeColumns);

            // Validate that the adapted permanences match the expected true permanences.
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                double[] perms = htmConnections.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(htmConnections.HtmConfig.NumInputs);
                for (int j = 0; j < truePermanences[i].Length; j++)
                {
                    Assert.IsTrue(Math.Abs(truePermanences[i][j] - perms[j]) <= 0.01);
                }
            }
        }


        /// <summary>
        /// Unit test for the 'AdaptSynapses' method when there are no active columns.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void TestAdaptSynapsesWithNoColumns()
        {
            // Initialization with default HtmConfig parameters
            var htmConfig = SetupHtmConfigDefaultParameters();
            htmConfig.InputDimensions = new int[] { 8 };
            htmConfig.ColumnDimensions = new int[] { 4 };
            htmConfig.SynPermInactiveDec = 0.01;
            htmConfig.SynPermActiveInc = 0.1;
            htmConfig.WrapAround = false;

            // Create Connections and SpatialPooler instances
            htmConnections = new Connections(htmConfig);
            SpatialPooler = new SpatialPooler();
            SpatialPooler.Init(htmConnections);

            // Set the minimum threshold value for synapse trimming
            htmConnections.HtmConfig.SynPermTrimThreshold = 0.01;

            // Define potential pools for each column
            int[][] potentialPools = new int[][] {
            new int[]{ 1, 1, 1, 1, 0, 0, 0, 0 },
            new int[]{ 1, 0, 0, 0, 1, 1, 0, 1 },
            new int[]{ 0, 0, 1, 0, 0, 0, 1, 0 },
            new int[]{ 1, 0, 0, 0, 0, 0, 1, 0 }
            };

            // Initialize permanences for each column
            double[][] permanences = new double[][] {
            new double[]{ 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450 },
            new double[]{ 0.000, 0.000, 0.014, 0.000, 0.000, 0.000, 0.110, 0.000 },
            new double[]{ 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            // Define the expected true permanences after adaptation
            double[][] truePermanences = new double[][] {
            new double[]{ 0.300, 0.110, 0.080, 0.140, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.250, 0.000, 0.000, 0.000, 0.280, 0.110, 0.000, 0.440 },
            new double[]{ 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.210, 0.000 },
            new double[]{ 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            // Set up proximal connected synapses and initial permanences for each column
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(potentialPools[i], (n) => (n == 1));
                htmConnections.GetColumn(i).SetProximalConnectedSynapsesForTest(htmConnections, indexes);
                htmConnections.GetColumn(i).SetPermanences(htmConnections.HtmConfig, permanences[i]);
            }

            // Define an input vector and an array of active columns (empty in this case)
            int[] inputVector = new int[] { 1, 0, 0, 1, 1, 0, 1, 0 };
            int[] activeColumns = new int[] { };

            // Execute the AdaptSynapses method with the specified parameters 
            SpatialPooler.AdaptSynapses(htmConnections, inputVector, activeColumns);

            // Validate that the dense permanences have been successfully adapted
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                double[] perms = htmConnections.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(htmConnections.HtmConfig.NumInputs);

                // Assert that the dense permanences are not null after adaptation
                Assert.IsNotNull(perms);
            }
        }


        /// <summary>
        /// Unit test for the AdaptSynapses method when there are no active columns and no input vectors.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void TestAdaptSynapsesWithNoColumnsNoInputVector()
        {
            // Initialize HtmConfig parameters
            var htmConfig = SetupHtmConfigDefaultParameters();
            htmConfig.InputDimensions = new int[] { 8 };
            htmConfig.ColumnDimensions = new int[] { 4 };
            htmConfig.SynPermInactiveDec = 0.01;
            htmConfig.SynPermActiveInc = 0.1;
            htmConfig.WrapAround = false;

            // Create Connections object and SpatialPooler instance
            htmConnections = new Connections(htmConfig);
            SpatialPooler = new SpatialPooler();
            SpatialPooler.Init(htmConnections);

            // Set minimum threshold value for synapse trimming
            htmConnections.HtmConfig.SynPermTrimThreshold = 0.01;

            // Define potential pools for each column
            int[][] potentialPools = new int[][] {
            new int[]{ 1, 1, 1, 1, 0, 0, 0, 0 },
            new int[]{ 1, 0, 0, 0, 1, 1, 0, 1 },
            new int[]{ 0, 0, 1, 0, 0, 0, 1, 0 },
            new int[]{ 1, 0, 0, 0, 0, 0, 1, 0 }
            };

            // Initialize permanences for each column
            double[][] permanences = new double[][] {
            new double[]{ 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450 },
            new double[]{ 0.000, 0.000, 0.014, 0.000, 0.000, 0.000, 0.110, 0.000 },
            new double[]{ 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            // Define true permanences after synapse adaptation
            double[][] truePermanences = new double[][] {
            new double[]{ 0.300, 0.110, 0.080, 0.140, 0.000, 0.000, 0.000, 0.000 },
            new double[]{ 0.250, 0.000, 0.000, 0.000, 0.280, 0.110, 0.000, 0.440 },
            new double[]{ 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.210, 0.000 },
            new double[]{ 0.040, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000 }
            };

            // Set up potential synapses and initial permanences for each column
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(potentialPools[i], (n) => (n == 1));
                htmConnections.GetColumn(i).SetProximalConnectedSynapsesForTest(htmConnections, indexes);
                htmConnections.GetColumn(i).SetPermanences(htmConnections.HtmConfig, permanences[i]);
            }

            // Define input vector and active columns
            int[] inputVector = new int[] { 1, 0, 0, 1, 1, 0, 1, 0 };
            int[] activeColumns = new int[] { };

            // Execute the AdaptSynapses method with specified parameters 
            SpatialPooler.AdaptSynapses(htmConnections, inputVector, activeColumns);

            // Assert that the resulting synapse permanences are not null for each column
            for (int i = 0; i < htmConnections.HtmConfig.NumColumns; i++)
            {
                double[] perms = htmConnections.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(htmConnections.HtmConfig.NumInputs);
                Assert.IsNotNull(perms);
            }
        }

        private const string CONNECTIONS_CANNOT_BE_NULL = "Connections cannot be null";
        private const string DISTALDENDRITE_CANNOT_BE_NULL = "Object reference not set to an instance of an object.";



        /// <summary>
        /// Testing whether the permanence of a synapse in a distal dendrite segment increases if its presynaptic cell 
        /// was active in the previous cycle.with a permanence value of 0.1. Then it calls the AdaptSegment 
        /// method with the presynaptic cells set to cn.GetCells(new int[] { 23, 37 }). This means that if 
        /// the presynaptic cell with index 23 was active in the previous cycle, the synapse's permanence 
        /// should be increased.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public void TestAdaptSegment_PermanenceStrengthened_IfPresynapticCellWasActive()
        {
            tm TemporalMemory = new tm();
            Connections Connections = new Connections();
            Parameters Parameters = Parameters.getAllDefaultParameters();
            Parameters.apply(Connections);
            TemporalMemory.Init(Connections);

            DistalDendrite dd = Connections.CreateDistalSegment(Connections.GetCell(0));
            Synapse s1 = Connections.CreateSynapse(dd, Connections.GetCell(23), 0.1);

            // Invoking AdaptSegments with only the cells with index 23
            /// whose presynaptic cell is considered to be Active in the
            /// previous cycle and presynaptic cell is Inactive for the cell 477
            tm.AdaptSegment(Connections, dd, Connections.GetCells(new int[] { 23 }), Connections.HtmConfig.PermanenceIncrement, Connections.HtmConfig.PermanenceDecrement);

            //Assert
            /// permanence is incremented for presynaptie cell 23 from 
            /// 0.1 to 0.2 as presynaptic cell was InActive in the previous cycle
            Assert.AreEqual(0.2, s1.Permanence);
        }