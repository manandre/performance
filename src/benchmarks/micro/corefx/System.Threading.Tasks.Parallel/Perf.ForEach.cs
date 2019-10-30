// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using MicroBenchmarks;

namespace System.Threading.Tasks.Tests
{
    [BenchmarkCategory(Categories.CoreFX)]
    public class ParallelForEachPerfTest
    {
        [Params(1, 1_000, 100_000)]
        public int count;

        public IEnumerable<object> Ranges()
        {
            yield return Enumerable.Range(0, count);
            yield return Enumerable.Range(0, count).ToArray();
            yield return Enumerable.Range(0, count).ToList();
        }

        [Benchmark]
        [ArgumentsSource(nameof(Ranges))]
        public void ParallelForEach(IEnumerable<int> range) => Parallel.ForEach(range, _ => { });
    }

    [BenchmarkCategory(Categories.CoreFX)]
    public class ParallelForEachOrderablePartitionerPerfTest
    {
        [Params(1, 1_000, 100_000)]
        public int count;

        public IEnumerable<object> Partitioners()
        {
            yield return Partitioner.Create(Enumerable.Range(0, count));
            yield return Partitioner.Create(Enumerable.Range(0, count).ToArray());
            yield return Partitioner.Create(Enumerable.Range(0, count).ToList());
        }

        [Benchmark]
        [ArgumentsSource(nameof(Partitioners))]
        public void ParallelForEach(OrderablePartitioner<int> partitioner) => Parallel.ForEach(partitioner, _ => { });
    }

    [BenchmarkCategory(Categories.CoreFX)]
    public class ParallelForEachPartitionerPerfTest
    {
        private Partitioner<int> _partitioner;

        [Params(1, 1_000, 100_000)]
        public int count;

        [GlobalSetup]
        public void Setup() => _partitioner = new MyPartitioner<int>(Enumerable.Range(0, count).ToList());

        [Benchmark]
        public void ParallelForEach() => Parallel.ForEach(_partitioner, _ => { });
    }
}