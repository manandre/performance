// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using BenchmarkDotNet.Attributes;
using MicroBenchmarks;

namespace System.Threading.Tasks.Tests
{
    [BenchmarkCategory(Categories.CoreFX)]
    public class ParallelInvokePerfTest
    {
        [Benchmark]
        public void ParallelInvokeSingle()
        {
            Parallel.Invoke(() => { });
        }

        private Action[] _actions;
        private ParallelOptions _options;

        [GlobalSetup(Targets = new[] { nameof(ParallelInvokeMultiple), nameof(ParallelInvokeMultipleNoParallelism) })]
        public void SetupParallelInvokeMultiple()
        {
            _actions = Enumerable.Range(0, 100).Select(_ => new Action(() => { })).ToArray();
            _options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
        }

        [Benchmark]
        public void ParallelInvokeMultiple()
        {
            Parallel.Invoke(_actions);
        }

        [Benchmark]
        public void ParallelInvokeMultipleNoParallelism()
        {
            Parallel.Invoke(_options, _actions);
        }
    }
}