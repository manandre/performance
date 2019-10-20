// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using BenchmarkDotNet.Attributes;
using MicroBenchmarks;

namespace System.Threading.Tasks.Tests
{
    [BenchmarkCategory(Categories.CoreFX)]
    public class ParallelForPerfTest
    {
        [Params(1, 1_000, 100_000)]
        public int count;

        [Benchmark]
        public void ParallelFor()
        {
            Parallel.For(0, count, _ => { });
        }
    }
}