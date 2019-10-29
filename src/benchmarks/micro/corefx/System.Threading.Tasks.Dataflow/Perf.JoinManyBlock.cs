// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using MicroBenchmarks;

namespace System.Threading.Tasks.Dataflow.Tests
{
    [BenchmarkCategory(Categories.CoreFX)]
    public class JoinManyBlockPerfTests : MultiTargetReceivableSourceBlockPerfTests<JoinManyBlock<int>, int[]>
    {
        public override JoinManyBlock<int> CreateBlock() => new JoinManyBlock<int>(10);

        protected override ITargetBlock<int>[] Targets => block.Targets;
    }
}