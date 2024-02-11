// Copyright (c) Damir Dobric. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using NeoCortexApi;
using NeoCortexApi.Entities;
using NeoCortexApi.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace UnitTestsProject
{
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







    }
}
