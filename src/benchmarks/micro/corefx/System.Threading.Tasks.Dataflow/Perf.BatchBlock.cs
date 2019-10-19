// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Threading.Tasks.Dataflow.Tests
{
    public class BatchBlockPerfTests : PropagatorPerfTests<IPropagatorBlock<int, int[]>, int[]>
    {
        protected override int ReceiveSize { get; } = 100;
        public override IPropagatorBlock<int, int[]> CreateBlock() => new BatchBlock<int>(ReceiveSize);
    }
}