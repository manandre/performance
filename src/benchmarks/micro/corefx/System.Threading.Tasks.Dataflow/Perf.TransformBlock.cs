// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using MicroBenchmarks;

namespace System.Threading.Tasks.Dataflow.Tests
{
    [BenchmarkCategory(Categories.CoreFX)]
    public class TransformBlockPerfTests : PropagatorPerfTests<IPropagatorBlock<int, int>, int>
    {
        public override IPropagatorBlock<int, int> CreateBlock() =>
            new TransformBlock<int, int>(i => i);

        #region Reactive Extensions
        [Benchmark(OperationsPerInvoke = 100_000)]
        public void RxPublishSubscribe()
        {
            var observer = block.AsObserver();
            var observable = block.AsObservable();
            observable.Subscribe(new IgnoreObserver<int>());
            for (int i = 0; i < 100_000; i++)
            {
                observer.OnNext(i);
            }
            observer.OnCompleted();
        }

        class IgnoreObserver<T> : IObserver<T>
        {
            public void OnCompleted() { }
            public void OnError(Exception error) { }
            public void OnNext(T value) { }
        }
        #endregion
    }

    public class ParallelTransformBlockPerfTests : PropagatorPerfTests<IPropagatorBlock<int, int>, int>
    {
        public override IPropagatorBlock<int, int> CreateBlock() =>
            new TransformBlock<int, int>(
                i => i,
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                }
            );
    }

    public class UnorderedParallelTransformBlockPerfTests : PropagatorPerfTests<IPropagatorBlock<int, int>, int>
    {
        public override IPropagatorBlock<int, int> CreateBlock() =>
            new TransformBlock<int, int>(
                i => i,
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    EnsureOrdered = false
                }
            );
    }

    public class EncapsulateBlockPerfTests : PropagatorPerfTests<IPropagatorBlock<int, int>, int>
    {
        public override IPropagatorBlock<int, int> CreateBlock()
        {
            var options = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            var block1 = new TransformBlock<int, int>(i => ++i, options);
            var block2 = new TransformBlock<int, int>(i => --i, options);
            block1.LinkTo(block2, new DataflowLinkOptions { PropagateCompletion = true });
            return DataflowBlock.Encapsulate(block1, block2);
        }
    }
}