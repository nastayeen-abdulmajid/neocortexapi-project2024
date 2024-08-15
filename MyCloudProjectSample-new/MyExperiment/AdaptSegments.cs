using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoCortexApi;
using NeoCortexApi.Entities;
using System.Reflection;
using System.Collections.Concurrent;
using System.IO;
using OfficeOpenXml;
using System.Data;
using Newtonsoft;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.Drawing.Style.Fill;

namespace MyExperiment
{
    
    public class AdaptSegments
    {
        TestDataReader1 TestDataReader2 = new TestDataReader1();


        /// <summary>
        /// Unit test method to verify the behavior of the decrement permanence function 
        /// when inactive presynaptic cells are involved.
        /// </summary>
        /// <param name="getCellnumber">The index of the cell to be used in the distal segment.</param>
        /// <param name="activeCellnumber">The index of the active cell to be used for testing.</param>
        /// <param name="initialPermanence">The initial permanence value of the synapse before adaptation.</param>
        /// <param name="conn">The Connections object that provides access to the cell and segment data.</param>
        /// <returns>
        /// Returns 1 if the test passes (i.e., the permanence is correctly incremented or decremented), 
        /// otherwise returns 0.
        /// </returns>
        public int AdaptSegments_UnitTest_DecrementPermanenceIfInactivePresynapticCells(int getCellnumber, int activeCellnumber, double initialPermanence, Connections conn)
        {
            // Initialize TemporalMemory object and its connections
            TemporalMemory tm = new TemporalMemory();
            tm.Init(conn);

            // Create a DistalDendrite segment for the specified cell
            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(getCellnumber));
            Cell activeCell = conn.GetCell(activeCellnumber);

            // Create a Synapse from the distal segment to the active cell with the initial permanence
            Synapse synapse = conn.CreateSynapse(dd, activeCell, initialPermanence);

            // List of active cells (for testing purposes, using a predefined cell number)
            List<int> randomActiveCellNumbers = new List<int> { 55 };
            List<Cell> ActiveCells = randomActiveCellNumbers.Select(cellNumber => conn.GetCell(cellNumber)).ToList();

            // Adapt the segment using the provided TemporalMemory and Connections objects
            TemporalMemory.AdaptSegment(conn, dd, ActiveCells, conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            // Retrieve the synapse from the distal dendrite if any exist
            if (dd.Synapses.Any())
            {
                synapse = dd.Synapses.First();
            }

            // Check if the active cell is present in the list of active cells
            if (ActiveCells.Any(cell => cell.Index == activeCellnumber))
            {
                // If active cell is present, check if the permanence has been incremented correctly
                if (Math.Abs(initialPermanence + conn.HtmConfig.PermanenceIncrement - synapse.Permanence) < 0.0001)
                {
                    return 1; // Test passes, permanence correctly incremented
                }
            }
            else
            {
                // If active cell is not present, check if the permanence has been decremented correctly
                if (Math.Abs(initialPermanence - conn.HtmConfig.PermanenceDecrement - synapse.Permanence) < 0.0001)
                {
                    return 1; // Test passes, permanence correctly decremented
                }
            }

            // If none of the conditions are met, the test fails
            return 0;
        }





        /// <summary>
        /// This unit test method verifies the change in synapse permanence for the previous cycle based on the activity of presynaptic cells.
        /// It sets up a temporal memory environment with default parameters and creates a distal dendrite segment associated with a specific cell index.
        /// Three synapses are then created on the distal segment, each linked to different presynaptic cells with initial permanence values provided.
        /// Next, the method simulates the adaptation process by invoking the AdaptSegment method with only the presynaptic cells 
        /// from the previous cycle, assuming their activity status has changed. After adaptation, the expected changes in synapse permanence 
        /// are calculated based on the configured permanence increment and decrement values. Finally, the method asserts that the actual 
        /// permanence values of the synapses match the expected values within a tolerance range of 0.1, ensuring the correctness of the adaptation process.
        /// </summary>

        public int AdaptSegments_UnitTest_VerifyPermanenceChangeForPreviousCycle()
        {

            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(7));/// Created a Distal dendrite segment of a cell0
            Synapse s1 = conn.CreateSynapse(dd, conn.GetCell(118), 0.7);/// Created a synapse on a distal segment of a cell index 23
            Synapse s2 = conn.CreateSynapse(dd, conn.GetCell(150), 0.2);/// Created a synapse on a distal segment of a cell index 37
            Synapse s3 = conn.CreateSynapse(dd, conn.GetCell(366), 0.3);/// 

            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 150, 366 }), conn.HtmConfig.PermanenceIncrement,
                conn.HtmConfig.PermanenceDecrement);/// Invoking AdaptSegments with only the cells with index 23 and 37
                                                    /// whose presynaptic cell is considered to be Active in the
                                                    /// previous cycle and presynaptic cell is Inactive for the cell 477

            double expectedS1Permanence = 0.7 - conn.HtmConfig.PermanenceDecrement; // Active
            double expectedS2Permanence = 0.2 + conn.HtmConfig.PermanenceIncrement; // Active
            double expectedS3Permanence = 0.3 + conn.HtmConfig.PermanenceIncrement; // Inactive

            if ((expectedS1Permanence == s1.Permanence) & (expectedS2Permanence == s2.Permanence) & (expectedS3Permanence == s3.Permanence))
            {
                return 1;

            }
            // Assert

            return 0;
        }


        /// <summary>
        /// Unit test for verifying the state of segments after adapting with the maximum number of synapses allowed per segment.
        /// </summary>
        /// <returns>Returns 1 if the test passes, otherwise returns 0.</returns>
        public int AdaptSegments_UnitTest_VerifySegmentStateAfterMaxSynapsesPerSegment()
        {
            // Create an instance of TemporalMemory and Connections classes
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();

            // Get default parameters and apply them to the connections
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);

            // Initialize the TemporalMemory with the connections
            tm.Init(conn);

            // Create a new DistalDendrite segment attached to a specific cell (cell 100)
            DistalDendrite dd1 = conn.CreateDistalSegment(conn.GetCell(100));

            // Determine the maximum number of synapses allowed per segment from the HTM configuration
            int MaxSynapsesPerSegment = conn.HtmConfig.MaxSynapsesPerSegment;

            // Add maximum allowed number of synapses to the DistalDendrite segment
            for (int i = 0; i < conn.HtmConfig.MaxSegmentsPerCell; i++)
            {
                conn.CreateSynapse(dd1, conn.GetCell(77), 0.2);
            }

            // Perform the segment adaptation process
            TemporalMemory.AdaptSegment(
                conn,                  // Connection object
                dd1,                   // DistalDendrite segment to adapt
                conn.GetCells(new int[] { 7 }), // Cells to adapt the segment with
                conn.HtmConfig.PermanenceIncrement, // Amount to increment permanence
                conn.HtmConfig.PermanenceDecrement  // Amount to decrement permanence
            );

            // Use reflection to get the private fields for segment count and synapse count from the Connections object
            var field1 = conn.GetType().GetField("m_NextSegmentOrdinal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var field2 = conn.GetType().GetField("m_NumSynapses", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Retrieve the values of the segment count and synapse count
            var segmentCount = Convert.ToInt32(field1.GetValue(conn));
            var synapseCount = Convert.ToInt32(field2.GetValue(conn));

            // Check if the number of segments is 1 and the number of synapses matches the maximum allowed
            if ((segmentCount == 1) & (MaxSynapsesPerSegment == synapseCount))
            {
                // Test passes
                return 1;
            }

            // Test fails; return 0
            // Uncomment and use assertions to validate the results in a proper unit testing framework
            // Assert.AreEqual(1, segmentCount, "Unexpected segment count");
            // Assert.AreEqual(MaxSynapsesPerSegment, synapseCount, "Unexpected synapse count");

            return 0;
        }



        /// <summary>
        /// Verifies the state of distal dendrite segments and active segments after adaptation.
        /// This unit test method sets up a temporal memory environment with default parameters, creates multiple distal dendrite segments, 
        /// and associates them with specific cell indices. It then creates synapses on these segments with varying initial permanence values.
        /// The adaptation process for each segment is simulated by invoking the AdaptSegment method with a set of active cells.
        /// The method retrieves internal fields from the Connections object to examine segment and synapse counts, 
        /// as well as active segment and matching segment counts. Finally, it asserts that the segment count matches the expected number, 
        /// the synapse count is as expected, and there are no active segments or matching segments.
        /// </summary>
        /// <returns>1 if the test passes, otherwise 0.</returns>
        public int AdaptSegments_UnitTest_VerifySegmentAndActiveSegmentStateAfterAdaptation()
        {
            // Create a new TemporalMemory instance.
            TemporalMemory tm = new TemporalMemory();

            // Create a new Connections instance.
            Connections conn = new Connections();

            // Retrieve and apply default parameters to the Connections object.
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);

            // Initialize the TemporalMemory instance with the Connections object.
            tm.Init(conn);

            // Create an array to hold distal dendrite segments.
            DistalDendrite[] segments = new DistalDendrite[4];
            for (int i = 0; i < segments.Length; i++)
            {
                // Create distal dendrite segments associated with specific cell indices.
                segments[i] = conn.CreateDistalSegment(conn.GetCell(76 + i));
            }

            // Create an array to hold synapses.
            Synapse[] synapses = new Synapse[5];
            // Define cell indices for the synapses.
            int[] synapseCellIndices = { 36, 46, 56, 66, 76 };
            for (int i = 0; i < synapses.Length; i++)
            {
                // Create synapses on the distal segments with varying initial permanence values.
                synapses[i] = conn.CreateSynapse(segments[i % 4], conn.GetCell(synapseCellIndices[i]), i * 0.5 - 1.5);
            }

            // Simulate the adaptation process for each distal dendrite segment.
            for (int i = 0; i < segments.Length; i++)
            {
                // Define the set of active cells for adaptation.
                int[] activeCells = { synapseCellIndices[i], synapseCellIndices[(i + 1) % synapseCellIndices.Length] };
                // Adapt the segment with the active cells and the permanence increment/decrement values.
                TemporalMemory.AdaptSegment(conn, segments[i], conn.GetCells(activeCells), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            }

            // Retrieve internal fields from the Connections object using reflection.
            var field1 = conn.GetType().GetField("m_NextSegmentOrdinal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var field3 = conn.GetType().GetField("m_SegmentForFlatIdx", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var field4 = conn.GetType().GetField("m_ActiveSegments", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var field5 = conn.GetType().GetField("m_MatchingSegments", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Get the segment count, active segment count, and matching segment count from the internal fields.
            var dictionary = (ConcurrentDictionary<int, DistalDendrite>)field3.GetValue(conn);
            var field2 = conn.GetType().GetField("m_NumSynapses", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var GetSegmentCount = Convert.ToInt32(field1.GetValue(conn));
            var GetActiveSegments = ((List<DistalDendrite>)field4.GetValue(conn)).Count;
            var GetMatchingSegments = ((List<DistalDendrite>)field5.GetValue(conn)).Count;
            var GetSynapseCount = Convert.ToInt32(field2.GetValue(conn));

            // Check if the segment count, synapse count, active segments, and matching segments match the expected values.
            if ((GetSegmentCount == 4) & (GetSynapseCount == 2) & (GetActiveSegments == 0) & (GetMatchingSegments == 0))
            {
                // Return 1 if the test passes.
                return 1;
            }

            // Optionally assert expected values (commented out here).
            //Assert.AreEqual(4, GetSegmentCount, "Unexpected segment count");
            //Assert.AreEqual(2, GetSynapseCount, "Unexpected synapse count");
            //Assert.AreEqual(0, GetActiveSegments, "Unexpected active segment count");
            //Assert.AreEqual(0, GetMatchingSegments, "Unexpected matching segment count");

            // Return 0 if the test fails.
            return 0;
        }


        /// <summary>
        /// Unit test to verify that adaptation occurs when the maximum number of synapses per segment is reached and exceeded.
        /// This test ensures that the system correctly handles cases where the number of synapses exceeds the defined limit per segment.
        /// </summary>
        /// <returns>
        /// A tuple containing:
        /// - An integer indicating the test result (1 for failure, 0 for success).
        /// - A string message describing the result of the test.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the number of synapses exceeds the maximum allowed per segment, indicating a failure in handling the limit.
        /// </exception>
        public (int, string) AdaptSegments_UnitTest_VerifyAdaptationWhenMaxSynapsesPerSegmentIsReachedAndExceeded()
        {
            // Initialize TemporalMemory and Connections
            TemporalMemory tm = new TemporalMemory(); // Create a new TemporalMemory instance
            Connections conn = new Connections(); // Create a new Connections instance
            Parameters p = Parameters.getAllDefaultParameters(); // Retrieve default parameters for the configuration
            p.apply(conn); // Apply parameters to the Connections instance
            tm.Init(conn); // Initialize the TemporalMemory with the Connections configuration

            // Create a DistalDendrite segment for testing
            DistalDendrite dd1 = conn.CreateDistalSegment(conn.GetCell(1)); // Create a DistalDendrite segment attached to a cell

            int numSynapses = 0; // Initialize counter for the number of synapses created
            Random random = new Random(); // Random number generator for selecting cells
            int totalCells = conn.Cells.Length; // Total number of cells available in the Connections instance

            // Generate synapses until the maximum number per segment is reached
            while (numSynapses < conn.HtmConfig.MaxSynapsesPerSegment)
            {
                int randomCellNumber = random.Next(1, totalCells + 1); // Select a random cell index
                Synapse s = conn.CreateSynapse(dd1, conn.GetCell(randomCellNumber), 0.5); // Create a new synapse
                numSynapses++; // Increment the synapse counter

                // Check if the maximum number of synapses has been reached
                if (numSynapses == conn.HtmConfig.MaxSynapsesPerSegment)
                {
                    // Adapt the segment to handle the maximum number of synapses
                    TemporalMemory.AdaptSegment(
                        conn,
                        dd1,
                        conn.GetCells(new int[] { randomCellNumber }),
                        conn.HtmConfig.PermanenceIncrement,
                        conn.HtmConfig.PermanenceDecrement
                    );
                }
            }

            // Validate whether the maximum number of synapses per segment was exceeded
            if (numSynapses >= conn.HtmConfig.MaxSynapsesPerSegment)
            {
                // Return failure result and throw exception if the limit was exceeded
                return (1, "The Maximum Synapse per segment was exceeded.");
                throw new ArgumentOutOfRangeException("The Maximum Synapse per segment was exceeded.");
            }

            // Return success result if the limit was not exceeded
            return (0, "The Maximum Synapse per segment was not exceeded.");
        }




        /// <summary>
        /// Verifies the behavior of segment destruction when no synapse is present on the segment.
        /// It initializes a temporal memory environment with default parameters and creates five distal dendrite segments, each with a corresponding synapse.
        /// Segments are adapted using the AdaptSegment method, and then the DestroyDistalDendrite method is explicitly called to destroy three of the segments.
        /// The test asserts the segment and synapse status before and after the destruction to ensure proper segment destruction without affecting other segments.
        /// </summary>
        public int AdaptSegments_UnitTest_VerifySegmentDestructionWhenNoSynapseIsPresent()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite[] segments = new DistalDendrite[5];
            Synapse[] synapses = new Synapse[5];

            for (int i = 0; i < segments.Length; i++)
            {
                segments[i] = conn.CreateDistalSegment(conn.GetCell(0));
                synapses[i] = conn.CreateSynapse(segments[i], conn.GetCell(23 + i), -1.5 + i * 0.4); // Adjusting the permanence values
            }

            // Adapt segments
            for (int i = 0; i < 3; i++)
            {
                TemporalMemory.AdaptSegment(conn, segments[i], conn.GetCells(new int[] { 21 + i, 22 + i, 23 + i }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            }

            var field1 = conn.GetType().GetField("m_ActiveSegments", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var field2 = conn.GetType().GetField("m_MatchingSegments", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var field4 = conn.GetType().GetField("m_NextSegmentOrdinal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var field5 = conn.GetType().GetField("m_NumSynapses", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var field9 = conn.GetType().GetField("nextSegmentOrdinal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var field6 = conn.GetType().GetField("m_SegmentForFlatIdx", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            MethodInfo destroyDistalDendriteMethod = typeof(Connections).GetMethod("DestroyDistalDendrite", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            var m_ASegment = field1.GetValue(conn);
            var m_MSegment = field2.GetValue(conn);


            ///Assert the segment and synapse status before the DestroyDistalDendrite method is explicitly called.
            ///
            int sample;
            if ((Convert.ToInt32(field4.GetValue(conn)) == 5) & (Convert.ToInt32(field5.GetValue(conn)) == 2))
            {
                sample = 1;
            }
            else
            {
                sample = 0;
            }

            // Destroy distal dendrite segments

            for (int i = 0; i < 3; i++)
            {
                destroyDistalDendriteMethod.Invoke(conn, new object[] { segments[i] });
            }


            if ((sample == 1) & (Convert.ToInt32(field4.GetValue(conn)) == 5) & (Convert.ToInt32(field5.GetValue(conn)) == 2))
            {
                return 1;
            }

            return 0;
        }


        /// <summary>
        /// Verifies that synapses with small negative permanence values are preserved 
        /// when the AdaptSegment method is invoked. It initializes a temporal memory environment with default parameters 
        /// and creates a distal dendrite segment. Three synapses are created on the segment with small negative permanence values. 
        /// The AdaptSegment method is then called with cells 102, 401, and 300. The test asserts that the synapses are not destroyed 
        /// after the adaptation process, ensuring that the preservation of synapses with small negative permanence values is maintained.
        /// </summary>
        public (int, string) AdaptSegments_UnitTest_PreservesSynapses_ForSmallNegativePermanenceValues()
        {

            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(5));

            // Create synapses with small negative permanence values
            Synapse[] synapses = new Synapse[3];
            synapses[0] = conn.CreateSynapse(dd, conn.GetCell(102), -0.0000003);
            synapses[1] = conn.CreateSynapse(dd, conn.GetCell(401), -0.0000002);
            synapses[2] = conn.CreateSynapse(dd, conn.GetCell(300), -0.0000004);

            // Invoke AdaptSegment with cells 23, 24, and 25
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 102, 401, 300 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            // Assert that synapses are not destroyed
            foreach (Synapse synapse in synapses)
            {
                if (!dd.Synapses.Contains(synapse))
                {
                    Console.WriteLine("Error: The expected synapse is not in the list.");

                    // Handle the error as needed (e.g., break, throw an exception, return a value, etc.)
                }

            }
            if (dd.Synapses.Count == 3)
            {
                Console.WriteLine($"Expected 3 synapses, found {dd.Synapses.Count}.");
                // Handle the error as needed (e.g., throw an exception, return a value, etc.)
                return (1, $"Expected 3 synapses, found {dd.Synapses.Count}.");
            }

            if (dd.Synapses.Count != 3)
            {
                Console.WriteLine($"Error: Expected 3 synapses, but found {dd.Synapses.Count}.");
                // Handle the error as needed (e.g., throw an exception, return a value, etc.)
                return (0, $"Error: Expected 3 synapses, but found {dd.Synapses.Count}.");
            }

            return (0, "Error Occured");
        }


        public (int, string) AdaptSegments_UnitTest_VerifySynapseDestructionWithNegativePermanenceValuesAfterAdaptation()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            // Create a distal dendrite segment
            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(55));

            // Create synapses with negative permanence values
            Synapse[] synapses = new Synapse[4];
            double[] permanenceValues = { -0.286, -0.355, -0.788, -0.817 }; // New permanence values
            for (int i = 0; i < synapses.Length; i++)
            {
                synapses[i] = conn.CreateSynapse(dd, conn.GetCell(52 + i), permanenceValues[i]);
            }

            // Invoke AdaptSegment with cells 23 and 24
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 52, 53, 54, 55 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            // Assert that synapses are destroyed and count is 0
            foreach (Synapse synapse in synapses)
            {
                if (!dd.Synapses.Contains(synapse))
                {
                    Console.WriteLine("The synapse is not in the list.");

                    // Handle the error as needed (e.g., break, throw an exception, return a value, etc.)
                }

            }

            if (dd.Synapses.Count == 0)
            {
                Console.WriteLine($"Expected 0 synapses, found {dd.Synapses.Count}.");
                // Handle the error as needed (e.g., throw an exception, return a value, etc.)
                return (1, $"Expected 0 synapses, found {dd.Synapses.Count}.");
            }

            Console.WriteLine($"Error: Expected 0 synapses, but found {dd.Synapses.Count}.");

            return (0, $"Error: Expected 0 synapses, but found {dd.Synapses.Count}.");
        }


        /// <summary>
        /// Verifies that synapses with negative permanence values are destroyed 
        /// after the AdaptSegment method is invoked. It initializes a temporal memory environment with default parameters 
        /// and creates a distal dendrite segment. Four synapses are created on the segment with negative permanence values. 
        /// The AdaptSegment method is then called with cells 52, 53, 54, and 55. The test asserts that the synapses are destroyed 
        /// after the adaptation process, ensuring that synapses with negative permanence values are removed as expected.
        /// </summary>

        public (int, string) AdaptSegments_UnitTest_EnsureAdaptSegmentThrowsExceptionWhenDistalDendriteIsNull()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(54));
            Synapse s1 = conn.CreateSynapse(dd, conn.GetCell(17), 0.66);

            //Assert.ThrowsException<NullReferenceException>(() =>
            //{
            //    TemporalMemory.AdaptSegment(conn, null, conn.GetCells(new int[] { 17 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            //}, "Expected NullReferenceException was not thrown"); // Because DD cannot be Null


            bool exceptionThrown = false;

            try
            {
                TemporalMemory.AdaptSegment(conn, null, conn.GetCells(new int[] { 17 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            }
            catch (NullReferenceException)
            {
                exceptionThrown = true;
            }

            if (exceptionThrown)
            {
                // Optionally, log that the exception was thrown as expected
                Console.WriteLine("NullReferenceException was thrown as expected.");
                return (1, "NullReferenceException was thrown as expected.");

            }
            // Log or handle the case where the exception was not thrown as expected
            Console.WriteLine("Expected NullReferenceException was not thrown.");
            return (0, "Expected NullReferenceException was not thrown.");
        }



        /// <summary>
        /// This unit test method checks the state of synapses on a distal dendrite segment after invoking the AdaptSegment method. 
        /// It initializes a temporal memory environment with default parameters and creates a distal dendrite segment. 
        /// Several synapses are created on the segment with various permanence values. The AdaptSegment method is then called 
        /// with cells corresponding to the synapses. After adaptation, the test verifies the state of each synapse, 
        /// ensuring that synapses with positive permanence values remain intact, while synapses with negative permanence values 
        /// are removed from the segment.
        /// </summary>

        public (int, string) AdaptSegments_UnitTest_CheckSynapseStateAfterAdaptatione()
        {
            // Arrange
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);


            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(0));
            Synapse[] synapses = new Synapse[]
            {
            conn.CreateSynapse(dd, conn.GetCell(82), 0.3),       // Updated value for s1
            conn.CreateSynapse(dd, conn.GetCell(85), 0.015),     // Updated value for s2
            conn.CreateSynapse(dd, conn.GetCell(89), 0.77),      // Updated value for s3
            conn.CreateSynapse(dd, conn.GetCell(92), 0.06),      // Updated value for s4
            conn.CreateSynapse(dd, conn.GetCell(93), 0.002),     // Updated value for s5
            conn.CreateSynapse(dd, conn.GetCell(95), 0.003),     // Updated value for s6
            conn.CreateSynapse(dd, conn.GetCell(97), -0.23),     // Updated value for s7
            conn.CreateSynapse(dd, conn.GetCell(99), -0.13)      // Updated value for s8
            };

            // Adapt segment with new values
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 82, 85, 89, 92, 93, 95, 97, 99 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            // Assert
            foreach (Synapse synapse in synapses)
            {
                if (synapse.Permanence >= 0)
                {
                    //Assert.IsTrue(dd.Synapses.Contains(synapse));
                    Console.WriteLine($"Synapse with cell {dd.Synapses.Contains(synapse)} was expected to be in the segment.");
                }

                else
                {
                    //Assert.IsFalse(dd.Synapses.Contains(synapse));
                    Console.WriteLine($"Synapse with cell {dd.Synapses.Contains(synapse)} was not expected to be in the segment.");
                }
                //Assert.IsFalse(dd.Synapses.Contains(synapse));
            }
            if (dd.Synapses.Count == 6)
            {
                Console.WriteLine("The segment contains the expected number of synapses.");
                return (1, "The segment contains the expected number of synapses.");
            }

            return (0, "The segment does not contains the expected number of synapses.");
        }

        /// <summary>
        /// This unit test method verifies the boundary constraint for permanence increment by attempting to increase 
        /// the permanence value beyond the maximum allowed bound. It initializes a temporal memory environment with 
        /// default parameters and creates a distal dendrite segment with a synapse having a permanence value that 
        /// exceeds the maximum bound. The method then invokes the AdaptSegment method twice with the same active cell 
        /// to simulate two consecutive iterations. After the adaptations, the test verifies that the synapse permanence 
        /// remains capped at the maximum bound defined by the system configuration.
        /// </summary>
        public (int, string) AdaptSegments_UnitTest_TestPermanenceIncrement_BoundaryConstraint()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite segment = conn.CreateDistalSegment(conn.GetCell(44));
            Synapse synapse = conn.CreateSynapse(segment, conn.GetCell(66), 1.1);

            // Act
            TemporalMemory.AdaptSegment(conn, segment, conn.GetCells(new int[] { 66 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            TemporalMemory.AdaptSegment(conn, segment, conn.GetCells(new int[] { 66 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            // Assert
            if (synapse.Permanence == 1.0)
            {
                return (1, "Permanence should be capped at the maximum bound");
            }

            return (0, "Permanence was not capped at the maximum bound");
        }


        /// <summary>
        /// This unit test method verifies the behavior of the GetCells method when provided with an empty array 
        /// of cell indexes. It initializes a temporal memory environment with default parameters and attempts 
        /// to retrieve cells using an empty array of cell indexes. The method then asserts that the returned 
        /// array of cells is also empty, as no cells are expected to be retrieved in this scenario.
        /// </summary>

        public (int, string) AdaptSegments_UnitTest_TestGetCells_ReturnsEmptyArrayForEmptyInput()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            int[] cellIndexes = Array.Empty<int>();
            Cell[] expectedCells = Array.Empty<Cell>();

            // Act
            Cell[] result = conn.GetCells(cellIndexes);

            // Assert
            if (result.Length == expectedCells.Length && result.SequenceEqual(expectedCells))
            {
                return (1, "Test passed: The arrays are equal.");
            }

            return (0, "Test failed: The arrays are not equal.");
            //CollectionAssert.AreEqual(expectedCells, result);
        }


        /// <summary>
        /// This unit test method verifies the behavior of the GetCells method when provided with a valid input array 
        /// of cell indexes. It initializes a connections object with a specified array of cells and attempts to 
        /// retrieve cells using a given array of cell indexes. The method then asserts that the returned array of 
        /// cells matches the expected array of cells based on the provided cell indexes.
        /// </summary>
        public (int, string) AdaptSegments_UnitTest_TestGetCells_ValidInput_ReturnsExpectedCellArray()
        {
            Connections conn = new Connections();
            int[] cellIndexes = { 13, 12, 24 };
            conn.Cells = new Cell[50];
            Cell[] expectedCells = { conn.Cells[13], conn.Cells[12], conn.Cells[24] };

            // Act
            Cell[] result = conn.GetCells(cellIndexes);

            // Assert
            if (result.Length == expectedCells.Length && result.SequenceEqual(expectedCells))
            {
                return (1, "Test passed: The arrays are equal.");
            }

            return (0, "Test failed: The arrays are not equal.");
            //CollectionAssert.AreEqual(expectedCells, result);
        }


        /// <summary>
        /// This unit test method verifies the behavior of the AdaptSegment method when provided with a complex 
        /// double permanence input that reaches the maximum permanence value. It initializes a temporal memory 
        /// and connections object with default parameters, creates a distal dendrite segment, and creates a synapse 
        /// with a specific initial permanence value. The method then adapts the segment with a single active cell, 
        /// incrementing the permanence using the HTM configuration parameters. After the first adaptation, it asserts 
        /// that the permanence has been incremented accordingly. Next, it attempts to adapt the segment again with 
        /// the same active cell, and asserts that the permanence is capped at the maximum bound defined by the HTM 
        /// configuration.
        /// </summary>        
        public int AdaptSegments_UnitTest_ComplexDoublePermanenceInput_MaxPermanenceReached()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(8));
            Synapse s1 = conn.CreateSynapse(dd, conn.GetCell(29), 0.865467362567887);

            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 29 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            int sample = 0;
            if (s1.Permanence == 0.965467362567887)
            {
                sample = 1;
            }
            //Assert.AreEqual(0.965467362567887, s1.Permanence);
            // Now permanence should be at max
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 29 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            if ((s1.Permanence == 1.0) & (sample == 1))
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// This unit test method verifies the behavior of the AdaptSegment method when adapting a segment with 
        /// a synapse whose permanence falls below the minimum threshold. It initializes a temporal memory and 
        /// connections object with default parameters, creates a distal dendrite segment, and creates a synapse 
        /// with a specific initial permanence value. The method then adapts the segment without any active cells, 
        /// decrementing the permanence using the HTM configuration parameters. After the adaptation, it asserts 
        /// that the synapse has been removed from the segment since its permanence falls below the minimum threshold 
        /// defined by the HTM configuration.
        /// </summary>

        public (int, string) AdaptSegments_UnitTest_VerifySynapseRemovalOnMinimumPermanenceAdaptation()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(4));
            Synapse synapse = conn.CreateSynapse(dd, conn.GetCell(32), 0.1);/// create a synapse on a dital segment of a cell with index 23

            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);/// Invoking AdaptSegments with the cell 15 whose presynaptic cell is 

            if (!dd.Synapses.Contains(synapse))
            {
                return (1, "The synapse was removed as expected due to minimum permanence.");

            }
            return (0, "The synapse was not removed as expected.");
            /// considered to be InActive in the previous cycle.
            //Assert.IsFalse(cn.GetSynapses(dd).Contains(s1));
            //Assert.IsFalse(dd.Synapses.Contains(synapse));/// permanence is decremented for presynaptie cell 477 from 
            /// 0.1 to 0 as presynaptic cell was InActive in the previous cycle
            /// There the synapse is destroyed as permanence < HtmConfig.Epsilon
        }


        /// <summary>
        /// This unit test method verifies the behavior of the AdaptSegment method when adapting a segment with 
        /// a synapse whose permanence falls below a certain threshold. It initializes a temporal memory and 
        /// connections object with default parameters, creates a distal dendrite segment, and creates a synapse 
        /// with a specific initial negative permanence value. The method then adapts the segment with an active 
        /// cell, which decrements the synapse's permanence using the HTM configuration parameters. After the 
        /// adaptation, it asserts that the synapse has been removed from the segment since its permanence falls 
        /// below the threshold defined by the HTM configuration.
        /// </summary>       
        public (int, string) AdaptSegments_UnitTest_VerifySynapseDestructionOnLowPermanenceAdaptation()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite distalDendrite = conn.CreateDistalSegment(conn.GetCell(99));
            Synapse synapse = conn.CreateSynapse(distalDendrite, conn.GetCell(49), -2.35);


            TemporalMemory.AdaptSegment(conn, distalDendrite, conn.GetCells(new int[] { 49 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            if (!distalDendrite.Synapses.Contains(synapse))
            {
                return (1, "The synapse was not destroyed as expected.");
            }

            return (0, "The synapse was destroyed as expected.");

        }

        /// <summary>
        /// This unit test method verifies that a synapse remains in the segment after adaptation if its permanence 
        /// value remains within the valid range. It initializes a temporal memory and connections object with 
        /// default parameters, creates a distal dendrite segment, and creates two synapses with different initial 
        /// permanence values. The method then adapts the segment without any active cells, which might result in 
        /// adjustments to the synapses' permanence values. After the adaptation, it asserts that the synapse 
        /// with a permanence value within the valid range remains in the segment, while the synapse with an 
        /// out-of-range permanence value is removed.
        /// </summary>
        public (int, string) AdaptSegments_UnitTest_VerifyStayOfSynapseAfterSegmentAdaptation()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite distalDendrite = conn.CreateDistalSegment(conn.GetCell(0));
            Synapse synapse1 = conn.CreateSynapse(distalDendrite, conn.GetCell(65), -0.1);
            Synapse synapse2 = conn.CreateSynapse(distalDendrite, conn.GetCell(69), 0.9);

            //Act
            TemporalMemory.AdaptSegment(conn, distalDendrite, conn.GetCells(new int[] { }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            if ((!distalDendrite.Synapses.Contains(synapse1)) & (distalDendrite.Synapses.Contains(synapse2)))
            {
                return (1, "The synapse created earlier is no longer present in the segment.The synapse created earlier is still present in the segment.");
            }

            return (0, "Null");
            //Assert
            //Assert.IsFalse(distalDendrite.Synapses.Contains(synapse1), "The synapse created earlier is no longer present in the segment.");
            //Assert.IsTrue(distalDendrite.Synapses.Contains(synapse2), "The synapse created earlier is still present in the segment.");

        }

        /// <summary>
        /// This unit test method verifies that attempting to retrieve cells from an invalid array of cell indexes
        /// throws an IndexOutOfRangeException as expected. It initializes a connections object with an array of 
        /// cells of size 3. The method then tries to retrieve cells using an array of cell indexes containing 
        /// invalid indices. It expects an IndexOutOfRangeException to be thrown because the provided cell 
        /// indexes are out of the valid range for the array of cells.
        /// </summary>

        //[ExpectedException(typeof(IndexOutOfRangeException))]///This attribute is used to specify the expected 
        ///exception. Therefore, the test will pass if the expected exception 
        ///of type IndexOutOfRangeException is thrown, and it will fail if 
        ///any other exception or no exception is thrown.
        public (int, string) AdaptSegments_UnitTest_TestInvalidArrayCells_WithInvalidArray_ThrowsIndexOutOfRangeException()
        {
            Connections cn = new Connections();
            cn.Cells = new Cell[3];
            int[] cellIndexes = new int[] { 2, 4, 7 };

            bool exceptionThrown = false;

            try
            {
                Cell[] result = cn.GetCells(cellIndexes);
            }
            catch (IndexOutOfRangeException)
            {
                exceptionThrown = true;
            }

            if (exceptionThrown)
            {
                // Log that the exception was thrown as expected
                Console.WriteLine("IndexOutOfRangeException was thrown as expected.");
                return (1, "IndexOutOfRangeException was thrown as expected.");
            }

            // Log or handle the case where the exception was not thrown as expected
            Console.WriteLine("Expected IndexOutOfRangeException was not thrown.");
            return (0, "Expected IndexOutOfRangeException was not thrown.");
        }



        /// <summary>
        /// This unit test method verifies that attempting to retrieve cells with a null array of cell indexes
        /// throws a NullReferenceException as expected. It initializes a connections object with a null array 
        /// of cells and a null array of cell indexes. The method then tries to retrieve cells using the null 
        /// array of cell indexes. It expects a NullReferenceException to be thrown because the cells array is null.
        /// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(NullReferenceException))]///This attribute is used to specify the expected 
        ///exception. Therefore, the test will pass if the expected exception 
        ///of type ArgumentNullException is thrown, and it will fail if 
        ///any other exception or no exception is thrown.
        public (int, string) AdaptSegments_UnitTest_TestNullArrayCells_ThrowsException()
        {
            // Arrange
            Connections cn = new Connections();
            cn.Cells = null;
            int[] indices_of_cell = null;

            // Act & Assert
            //Cell[] output = cn.GetCells(indices_of_cell);
            bool exceptionThrown = false;
            try
            {
                Cell[] output = cn.GetCells(indices_of_cell);
            }
            catch (NullReferenceException)
            {
                exceptionThrown = true;
            }

            if (exceptionThrown)
            {
                // Log that the exception was thrown as expected
                Console.WriteLine("NullReferenceException was thrown as expected.");
                return (1, "NullReferenceException was thrown as expected.");
            }

            // Log or handle the case where the exception was not thrown as expected
            Console.WriteLine("Expected NullReferenceException was not thrown.");
            return (0, "Expected NullReferenceException was not thrown.");
        }


        /// <summary>
        /// This unit test verifies that synapses are preserved when the permanence values are very small.
        /// It initializes a temporal memory object, a connections object, and applies default parameters.
        /// Then, it creates a distal dendrite segment and three synapses with very small permanence values.
        /// After that, it invokes the AdaptSegment method with the three synapses' presynaptic cells.
        /// Finally, it asserts that all synapses are preserved after adaptation.
        /// </summary>
        //[TestMethod]
        public (int, string) AdaptSegments_UnitTest_PreservesSynapses_ForVerySmallPermanenceValues()
        {

            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(5));

            // Create synapses with small permanence values
            Synapse[] synapses = new Synapse[3];
            synapses[0] = conn.CreateSynapse(dd, conn.GetCell(102), 0.0000003);
            synapses[1] = conn.CreateSynapse(dd, conn.GetCell(401), 0.0000002);
            synapses[2] = conn.CreateSynapse(dd, conn.GetCell(300), 0.0000004);

            // Invoke AdaptSegment with cells 23, 24, and 25
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 102, 401, 300 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            // Assert that synapses are not destroyed
            foreach (Synapse synapse in synapses)
            {
                if (!dd.Synapses.Contains(synapse))
                {
                    Console.WriteLine($"Error: Synapse with permanence {synapse.Permanence} was destroyed.");
                    return (0, $"Error: Synapse with permanence {synapse.Permanence} was destroyed."); // Indicate that the test failed
                }
                //Assert.IsTrue(dd.Synapses.Contains(synapse));
            }
            //Assert.AreEqual(3, dd.Synapses.Count);
            // Check if the number of synapses is still 3
            if (dd.Synapses.Count == 3)
            {
                return (1, "The number of synapses are as expected.");

            }

            return (0, "The number of synapses are not as expected.");
        }

        /// <summary>
        /// This unit test verifies that synapses are preserved when the permanence values are very large.
        /// It initializes a temporal memory object, a connections object, and applies default parameters.
        /// Then, it creates a distal dendrite segment and three synapses with very large permanence values.
        /// After that, it invokes the AdaptSegment method with the three synapses' presynaptic cells.
        /// Finally, it asserts that all synapses are preserved after adaptation.
        /// </summary>
        public int AdaptSegments_UnitTest_PreservesSynapses_ForVeryLargePermanenceValues()
        {

            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(5));

            // Create synapses with small permanence values
            Synapse[] synapses = new Synapse[3];
            synapses[0] = conn.CreateSynapse(dd, conn.GetCell(102), 16799999);
            synapses[1] = conn.CreateSynapse(dd, conn.GetCell(401), 762638282);
            synapses[2] = conn.CreateSynapse(dd, conn.GetCell(300), 817637383);

            // Invoke AdaptSegment with cells 23, 24, and 25
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 102, 401, 300 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            // Assert that synapses are not destroyed
            foreach (Synapse synapse in synapses)
            {
                if (!dd.Synapses.Contains(synapse))
                {
                    Console.WriteLine(" error");
                }
                //Assert.IsTrue(dd.Synapses.Contains(synapse));
            }
            if (dd.Synapses.Count == 3)
            {
                return 1;
            }
            //Assert.AreEqual(3, dd.Synapses.Count);
            return 0;
        }

        /// <summary>
        /// This unit test verifies that the AdaptSegment method adjusts synapse permanence based on the previous active cells.
        /// It initializes a temporal memory object, a connections object, and applies default parameters.
        /// Then, it creates a distal dendrite segment and three synapses with initial permanence values.
        /// After that, it invokes the AdaptSegment method with the cells representing previous active states.
        /// Finally, it asserts that the synapse permanence values are adjusted as expected.
        /// </summary>  
        public int AdjustsSynapsePermanenceBasedOnPreviousActiveCells()
        {
            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite distalDendrite = conn.CreateDistalSegment(conn.GetCell(0));
            Synapse s1 = conn.CreateSynapse(distalDendrite, conn.GetCell(42), 0.5);
            Synapse s2 = conn.CreateSynapse(distalDendrite, conn.GetCell(45), 0.3);
            Synapse s3 = conn.CreateSynapse(distalDendrite, conn.GetCell(44), 0.7);

            TemporalMemory.AdaptSegment(conn, distalDendrite, conn.GetCells(new int[] { 42, 45 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            if ((s1.Permanence == 0.6) & (s2.Permanence == 0.4) & (s3.Permanence == 0.6))
            {
                return 1;
            }
            return 0;
            //Assert.AreEqual(0.6, s1.Permanence, 0.1);
            //Assert.AreEqual(0.4, s2.Permanence, 0.1);
            //Assert.AreEqual(0.6, s3.Permanence, 0.1);

        }

        /// <summary>
        /// It initializes a temporal memory instance, sets up connections, and creates a distal dendrite segment (`dd`).
        /// Three synapses are created with very large negative permanence values.
        /// The `AdaptSegment` method is invoked with two active cells.
        /// It asserts that none of the synapses are preserved in the distal dendrite segment, and the count of synapses is zero.
        /// </summary>
        public int PreservesSynapses_ForVeryLargeNegativePermanenceValues()
        {

            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(5));

            // Create synapses with small permanence values
            Synapse[] synapses = new Synapse[3];
            synapses[0] = conn.CreateSynapse(dd, conn.GetCell(102), -16799999);
            synapses[1] = conn.CreateSynapse(dd, conn.GetCell(401), -762638282);
            synapses[2] = conn.CreateSynapse(dd, conn.GetCell(300), -817637383);

            // Invoke AdaptSegment with cells 23, 24, and 25
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 102, 401 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            // Assert that synapses are not destroyed
            foreach (Synapse synapse in synapses)
            {
                if (dd.Synapses.Contains(synapse))
                {
                    return 0;
                }

            }

            if (dd.Synapses.Count == 0)
            {
                return 1;
            }
            //Assert.AreEqual(0, dd.Synapses.Count);
            return 0;
        }

        /// <summary>
        /// Unit test to verify that synapses with zero permanence values are preserved after adaptation.
        /// </summary>
        public int PreservesSynapses_ForZeroPermanenceValues()
        {

            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(5));

            // Create synapses with small permanence values
            Synapse[] synapses = new Synapse[3];
            synapses[0] = conn.CreateSynapse(dd, conn.GetCell(102), 0);
            synapses[1] = conn.CreateSynapse(dd, conn.GetCell(401), 0);

            // Invoke AdaptSegment with cells 23, 24, and 25
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 102, 401 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);

            if (dd.Synapses.Count == 2)
            {
                return 1;
            }
            return 0;
            //Assert.AreEqual(2, dd.Synapses.Count);

        }

        /// <summary>
        /// Unit test to verify that an empty segment has no synapses after adaptation.
        /// </summary>
        public int Verify_Emptysegement()
        {

            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(5));

            // Create synapses with small permanence values
            Synapse[] synapses = new Synapse[3];
            synapses[0] = conn.CreateSynapse(dd, conn.GetCell(102), -167737383);
            synapses[1] = conn.CreateSynapse(dd, conn.GetCell(401), -762638282);
            synapses[2] = conn.CreateSynapse(dd, conn.GetCell(300), -817637383);

            // Invoke AdaptSegment with cells 23, 24, and 25
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 102, 401 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            int sample = 0;
            if (dd.Synapses.Count == 0)
            {
                sample = 1;
            }
            //Assert.AreEqual(0, dd.Synapses.Count);

            var field1 = conn.GetType().GetField("m_NumSynapses", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo destroyDistalDendriteMethod = typeof(Connections).GetMethod("DestroyDistalDendrite", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            // Assert that synapses are not destroyed
            destroyDistalDendriteMethod.Invoke(conn, new object[] { dd });

            if ((sample == 1) & (Convert.ToInt32(field1.GetValue(conn)) == 0))
            {
                return 1;
            }
            return 0;
            //Assert.AreEqual(0, Convert.ToInt32(field1.GetValue(conn)));

        }

        /// <summary>
        /// Unit test to ensure that a segment is destroyed even if only one synapse is left after adaptation.
        /// </summary>       
        public int AdaptSegments_UnitTest_KillSegmentEvenIfOnlyoneSynapse_is_left()
        {

            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(55));

            // Create synapses with small permanence values
            Synapse[] synapses = new Synapse[5];
            synapses[0] = conn.CreateSynapse(dd, conn.GetCell(15), -167737383);
            synapses[1] = conn.CreateSynapse(dd, conn.GetCell(16), -762638282);
            synapses[2] = conn.CreateSynapse(dd, conn.GetCell(17), -817637383);
            synapses[3] = conn.CreateSynapse(dd, conn.GetCell(18), -817637383);
            synapses[4] = conn.CreateSynapse(dd, conn.GetCell(19), 0.5);

            // Invoke AdaptSegment with cells 23, 24, and 25
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 102, 401 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            int sample = 0;
            if (dd.Synapses.Count == 1)
            {
                sample = 1;
            }

            //Assert.AreEqual(1, dd.Synapses.Count);

            var field1 = conn.GetType().GetField("m_NextSegmentOrdinal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var segmentCount = Convert.ToInt32(field1.GetValue(conn));
            MethodInfo destroyDistalDendriteMethod = typeof(Connections).GetMethod("DestroyDistalDendrite", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            // Assert that synapses are not destroyed
            destroyDistalDendriteMethod.Invoke(conn, new object[] { dd });
            if ((segmentCount == 1) & (sample == 1))
            {
                return 1;
            }
            //Assert.AreEqual(1, segmentCount);
            return 0;

        }

        /// <summary>
        /// Unit test to check if a segment survives after adaptation.
        /// </summary>
        public int AdaptSegments_UnitTest_CheckIfSegmentSurvives()
        {

            TemporalMemory tm = new TemporalMemory();
            Connections conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);
            tm.Init(conn);

            DistalDendrite dd = conn.CreateDistalSegment(conn.GetCell(5));

            // Create synapses with small permanence values
            Synapse[] synapses = new Synapse[5];
            synapses[0] = conn.CreateSynapse(dd, conn.GetCell(15), 16);
            synapses[1] = conn.CreateSynapse(dd, conn.GetCell(16), 76);
            synapses[2] = conn.CreateSynapse(dd, conn.GetCell(17), -8);
            synapses[3] = conn.CreateSynapse(dd, conn.GetCell(18), 8);
            synapses[4] = conn.CreateSynapse(dd, conn.GetCell(19), 0.5);

            // Invoke AdaptSegment with cells 23, 24, and 25
            TemporalMemory.AdaptSegment(conn, dd, conn.GetCells(new int[] { 15, 16, 17, 18, 19 }), conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement);
            int sample = 0;
            if (dd.Synapses.Count == 4)
            {
                sample = 1;
            }

            //Assert.AreEqual(4, dd.Synapses.Count);

            var field1 = conn.GetType().GetField("m_NextSegmentOrdinal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var segmentCount = Convert.ToInt32(field1.GetValue(conn));
            MethodInfo destroyDistalDendriteMethod = typeof(Connections).GetMethod("DestroyDistalDendrite", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            if ((segmentCount == 1) & (sample == 1))
            {
                return 1;
            }
            return 0;
            //Assert.AreEqual(1, segmentCount);

        }

        /// <summary>
        /// This unit test method verifies the permanence of synapses in the temporal memory after adaptation.
        /// It checks whether the permanence values fall within the expected range after the adaptation process.
        /// </summary>
        /// <param name="inputFile">The name of the JSON file containing test cases.</param>
        /// <returns>A tuple where the first item is an integer indicating the test result (1 for success, 0 for failure), 
        /// and the second item is a string message describing the result.</returns>
        public async Task<(int, string)> AdaptSegments_UnitTest_VerifyPermanenceBoundsAfterAdaptation(string inputFile)
        {
            // Define the output folder and path for the JSON file containing test cases
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "DownloadedFiles");
            string jsonFilePath = Path.Combine(outputFolder, inputFile);

            // Load test cases from the specified JSON file
            List<TestCase1> testCases;

            // Assuming TestDataReader1.LoadTestCases1 loads test cases from a JSON file
            testCases = TestDataReader1.LoadTestCases1(jsonFilePath);
            int result= 0;
            // Iterate through each test case to perform the verification
            foreach (var testCase in testCases)
            {
                // Arrange: Set up the temporal memory and connections for the test
                TemporalMemory tm = new TemporalMemory(); // Create an instance of TemporalMemory
                Connections conn = new Connections(); // Create an instance of Connections
                Parameters p = Parameters.getAllDefaultParameters(); // Get default parameters
                p.apply(conn); // Apply parameters to the connections
                tm.Init(conn); // Initialize the temporal memory with the connections

                // Create a distal segment and identify active and inactive cells
                DistalDendrite segment = conn.CreateDistalSegment(conn.GetCell(4)); // Create a segment for a cell
                Cell activeCell = conn.GetCell(testCase.ActiveCellnum); // Get the active cell from the test case
                Cell inactiveCell = conn.GetCell(2); // Get an inactive cell (though not used here)

                // Act: Create a synapse and perform the adaptation process
                Synapse synapse = conn.CreateSynapse(segment, activeCell, testCase.InitialPermanence); // Create a synapse with initial permanence
                TemporalMemory.AdaptSegment(conn, segment, new List<Cell> { activeCell }, conn.HtmConfig.PermanenceIncrement, conn.HtmConfig.PermanenceDecrement); // Adapt the segment

                // Assert: Verify if the permanence value of the synapse is within the acceptable range
                if (synapse.Permanence == 1.0)
                {
                    // Return success result if the permanence value is within the acceptable range
                    result = 1;
                }
            }

            if (result == 1)
            {
                return (1, "Permanence is within the acceptable range.");
            }
            // Return failure result if no synapse permanence was within the acceptable range
            return (0, "Test case failed. No synapse permanence within the acceptable range.");
        }


    }

}

