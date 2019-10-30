// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCOREAPP2_1 && !NETCOREAPP2_2 && !NETFRAMEWORK
using System.Collections.Generic;
#endif

namespace System.Threading.Tasks.Dataflow.Tests
{
    public class TransformManyBlockPerfTests : ReceivablePropagatorPerfTests<TransformManyBlock<int, int>, int>
    {
        public override TransformManyBlock<int, int> CreateBlock() =>
            new TransformManyBlock<int, int>(
                i => new int[] { i },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                });
    }

#if !NETCOREAPP2_1 && !NETCOREAPP2_2 && !NETFRAMEWORK
    public class AsyncEnumerableTransformManyBlockPerfTests : ReceivablePropagatorPerfTests<TransformManyBlock<int, int>, int>
    {
        public override TransformManyBlock<int, int> CreateBlock() =>
            new TransformManyBlock<int, int>(
                ToAsyncEnumerable,
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                });

        async IAsyncEnumerable<T> ToAsyncEnumerable<T>(T i)
        {
            await Task.Yield();
            yield return i;
        }
    }
#endif
}