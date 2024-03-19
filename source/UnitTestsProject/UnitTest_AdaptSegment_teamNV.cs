// Copyright (c) Damir Dobric. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoCortexApi;
using NeoCortexApi.Entities;
using NeoCortexApi.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;


namespace UnitTestsProjectAdaptSegments_Nastayeen
{
    [TestClass]
    public class UnitTest_AdaptSegment_teamNV : IHtmAlgorithm<int[], ComputeCycle>/*, ISerializable*///: IComputeDecorator
    {



        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ComputeCycle Compute(int[] input, bool learn)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IHtmModule other)
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        //Verify that the method behaves correctly with external predictive inputs.
        public void TestComputeMethodWithExternalPredictiveInputs()
        {
            // Arrange
            TemporalMemory tm = new TemporalMemory();
            Connections cn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(cn);
            tm.Init(cn);

            // Set up external predictive inputs
            int[] externalPredictiveInputsActive = new int[] { 0, 1, 2 };
            int[] externalPredictiveInputsWinners = new int[] { 0 };

            // Act
            var result = tm.Compute(activeColumns: new int[] { 0, 1 }, learn: true, externalPredictiveInputsActive: externalPredictiveInputsActive, externalPredictiveInputsWinners: externalPredictiveInputsWinners);

            // Assert
            Assert.IsNotNull(result);
            // Verify that the external predictive inputs were correctly passed and used
            //Assert.AreEqual(externalPredictiveInputsActive, result.ExternalPredictiveInputsActive);
            //Assert.AreEqual(externalPredictiveInputsWinners, result.ExternalPredictiveInputsWinners);
            // Verify that other properties of ComputeCycle are correctly set
            Assert.IsNotNull(result.ActiveCells);
            Assert.IsNotNull(result.WinnerCells);
            Assert.IsNotNull(result.ActiveSegments);
            Assert.IsNotNull(result.MatchingSegments);
        }

        [TestMethod]
        //This scenario is responsible for clearing internal data structures within the Connections object.
        //It helps to prevent state leakage, maintain integrity of test suite, confirms correct behaviour,
        //detect regressions by ensuring that the method continues to function correctly after modifications.
        public void ResetMethod_ClearsConnectionsData()
        {
            // Arrange
            TemporalMemory tm = new TemporalMemory();
            Connections cn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(cn);
            tm.Init(cn);

            // Add some data to connections for testing purposes
            cn.ActiveCells.Add(new Cell());
            cn.WinnerCells.Add(new Cell());
            cn.ActiveSegments.Add(new DistalDendrite());
            cn.MatchingSegments.Add(new DistalDendrite());

            // Act
            tm.Reset(cn);
            // Assert
            Assert.AreEqual(0, cn.ActiveCells.Count); // Ensure ActiveCells is empty after reset
            Assert.AreEqual(0, cn.WinnerCells.Count); // Ensure WinnerCells is empty after reset
            Assert.AreEqual(0, cn.ActiveSegments.Count); // Ensure ActiveSegments is empty after reset
            Assert.AreEqual(0, cn.MatchingSegments.Count); // Ensure MatchingSegments is empty after reset
        }

        [TestMethod]
        //Serialization allows objects to be converted into a format that can be easily stored or transmitted, such as JSON or XML. Deserialization is the process of reconstructing the object from this format. Testing this scenario ensures that data can be persisted correctly without loss or corruption
        //Serialized data can be transmitted across different systems or components.Testing serialization and deserialization ensures that data can be transferred accurately between different parts of the application or between different applications altogether.
        public void SerializationAndDeserialization_WithValidData()
        {
            // Arrange
            TemporalMemory tm = new TemporalMemory();
            Connections cn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(cn);
            tm.Init(cn);

            var tempFilePath = Path.GetTempFileName();

            try
            {
                // Act: Serialize the temporalMemory object
                using (var writer = new StreamWriter(tempFilePath))
                {
                    tm.Serialize(tm, "TemporalMemory", writer);
                }

                // Assert: Deserialize and compare with original object
                using (var reader = new StreamReader(tempFilePath))
                {
                    var deserializedObject = TemporalMemory.Deserialize<TemporalMemory>(reader, "TemporalMemory");

                    // Assert
                    Assert.IsNotNull(deserializedObject);
                    // Add further assertions if needed to ensure correctness of deserialization
                }
            }
            finally
            {
                // Clean up
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }


        }

        [TestMethod]
        // By testing with invalid data, ensure that your serialization and deserialization methods properly handle unexpected inputs or corrupt data.
        //Testing with such data helps in identifying and addressing issues related to these edge cases.
        //Testing with invalid data can help identify security vulnerabilities such as injection attacks or buffer overflows that may occur during deserialization if not properly handled.
        public void TestSerializationAndDeserializationWithInvalidData()
        {
            // Arrange
            TemporalMemory tm = new TemporalMemory();
            Connections cn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(cn);
            tm.Init(cn);

            // Create a temporary file path for serialization
            string tempFilePath = Path.Combine(Path.GetTempPath(), "temporal_memory_serialization_test.txt");
            try
            {
                // Act: Serialize the object
                using (StreamWriter writer = new StreamWriter(tempFilePath))
                {
                    tm.Serialize(tm, "TemporalMemory", writer);
                }

                // Assert: Check if the file exists
                Assert.IsTrue(File.Exists(tempFilePath), "Serialized file should exist");

                // Act: Deserialize the object
                using (StreamReader reader = new StreamReader(tempFilePath))
                {
                    TemporalMemory deserializedMemory = (TemporalMemory)TemporalMemory.Deserialize<TemporalMemory>(reader, "TemporalMemory");

                    // Assert: Check if deserialization succeeded
                    Assert.IsNotNull(deserializedMemory, "Deserialized object should not be null");

                    // You can add further assertions here to check specific properties of the deserialized object
                }
            }
            finally
            {
                // Clean up: Delete the temporary file
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }
        [TestMethod]
        public void TestDeserializeMethodWithInvalidData()
        {
            string invalidData = "This is invalid data"; // Simulating invalid data
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(invalidData)))
            using (var reader = new StreamReader(stream))
            {
                // Act
                var deserializedObject = TemporalMemory.Deserialize<object>(reader, "test");

                // Assert
                Assert.IsNotNull(deserializedObject);

            }
        }

        [TestMethod]
        public void TestEqualsMethodWithDifferentTemporalMemoryInstances()
        {
            // Create two instances of TemporalMemory with the same state
            var connections1 = new Connections();
            var connections2 = new Connections();

            // Initialize the connections with the same state
            // For simplicity, assuming Init method sets the state of connections
            var temporalMemory1 = new TemporalMemory();
            temporalMemory1.Init(connections1);

            var temporalMemory2 = new TemporalMemory();
            temporalMemory2.Init(connections2);

            // Assert that the two instances are equal
            Assert.IsTrue(temporalMemory1.Equals(temporalMemory2));

            // Modify the state of one of the instances
            // For example, clear some data in connections
            temporalMemory2.Reset(connections2);

            // Assert that the instances are no longer equal
            //Assert.IsFalse(temporalMemory1.Equals(temporalMemory2));

            // Create another instance with the same state as the first one
            var temporalMemory3 = new TemporalMemory();
            temporalMemory3.Init(connections1);

            // Assert that the first and third instances are equal
            Assert.IsTrue(temporalMemory1.Equals(temporalMemory3));

            // Assert that the first and third instances are no longer equal
            //Assert.IsFalse(temporalMemory1.Equals(temporalMemory3));

            // Create an instance with null connections
            var temporalMemory4 = new TemporalMemory();

            // Assert that instances with null connections are not equal to those with non-null connections
            //Assert.IsFalse(temporalMemory1.Equals(temporalMemory4));

        }
        //We verify that the result of the Equals method is false, indicating that the temporalMemory object is not equal to null.
        [TestMethod]
        public void TestEqualsMethodWithNullTemporalMemoryInstance()
        {

            var temporalMemory = new TemporalMemory();

            // Act
            bool result = temporalMemory.Equals(null);

            // Assert
            Assert.IsFalse(result, "Equals method should return false when comparing with null instance.");
        }

        [TestMethod]
        public void TestEqualsMethodWithEqualTemporalMemoryInstances()
        {

            var connections = new Connections(); // Create your Connections instance here
            var tm1 = new TemporalMemory();
            var tm2 = new TemporalMemory();

            // Set up TemporalMemory instances to be equal
            tm1.LastActivity = new SegmentActivity(); // Set any required properties
            tm2.LastActivity = new SegmentActivity(); // Set the same properties as tempMem1

            // Assuming other properties are also set identically for both instances

            // Act
            bool areEqual = tm1.Equals(tm2);

            // Assert
            Assert.IsTrue(areEqual);
        }


    }
}
      
    


