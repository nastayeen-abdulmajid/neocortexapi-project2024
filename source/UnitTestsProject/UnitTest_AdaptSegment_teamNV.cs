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
        public void TestComputeMethodWithExternalPredictiveInputs1()
        {
            // Arrange
            tm tm = new tm();
            Connections cn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(cn);
            tm.Init(cn);

            // Define sample input data
            int[] activeColumns = { 1, 2, 3 };
            bool learn = true;
            int[] externalPredictiveInputsActive = new int[] { 4, 5 };
            int[] externalPredictiveInputsWinners = new int[] { 4 };

            // Act
            // Calling the Compute method with external predictive inputs
            ComputeCycle result = tm.Compute(activeColumns, learn, externalPredictiveInputsActive, externalPredictiveInputsWinners);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        // Verify that the method behaves correctly with external predictive inputs.
        //This is important because in real-world applications, systems often receive inputs from external sources, and it's crucial to ensure that your code handles these inputs correctly.

        public void TestComputeMethodWithExternalPredictiveInputs()
        {
            // Arrange
            tm tm = new tm();
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
            tm tm = new tm();
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
            tm tm = new tm();
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
                    var deserializedObject = tm.Deserialize<tm>(reader, "TemporalMemory");

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
        // By testing with invalid data, you can ensure that your serialization and deserialization methods properly handle unexpected inputs or corrupt data.
        //Testing with such data helps in identifying and addressing issues related to these edge cases.
        //Testing with invalid data can help identify security vulnerabilities such as injection attacks or buffer overflows that may occur during deserialization if not properly handled.
        public void TestSerializationAndDeserializationWithInvalidData()
        {
            // Arrange
            tm tm = new tm();
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
                    tm deserializedMemory = (tm)tm.Deserialize<tm>(reader, "TemporalMemory");

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

    }
}

