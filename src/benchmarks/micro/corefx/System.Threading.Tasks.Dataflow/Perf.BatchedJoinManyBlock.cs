// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using MicroBenchmarks;

namespace System.Threading.Tasks.Dataflow.Tests
{
    [BenchmarkCategory(Categories.CoreFX)]
    public class BatchedJoinManyBlockPerfTests : MultiTargetReceivableSourceBlockPerfTests<BatchedJoinManyBlock<int>, IList<int>[]>
    {
        protected override int ReceiveSize { get; } = 100;

        public override BatchedJoinManyBlock<int> CreateBlock() => new BatchedJoinManyBlock<int>(ReceiveSize, 10);

        protected override ITargetBlock<int>[] Targets => block.Targets;
    }
}