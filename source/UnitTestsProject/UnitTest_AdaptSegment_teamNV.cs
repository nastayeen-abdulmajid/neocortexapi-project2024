// Copyright (c) Damir Dobric. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoCortexApi;
using NeoCortexApi.Entities;
using NeoCortexApi.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


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
        public void TestComputeMethodWithExternalPredictiveInputs()
        {
            // Arrange
            TemporalMemory tm = new TemporalMemory();
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



    }





}

