// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System.Threading.Tasks.Tests
{
    //
    // Utility class for use w/ Partitioner-style ForEach testing.
    // Created by Cindy Song.
    //
    public class MyPartitioner<TSource> : Partitioner<TSource>
    {
        private IList<TSource> _data;

        public MyPartitioner(IList<TSource> data)
        {
            _data = data;
        }

        public override IList<IEnumerator<TSource>> GetPartitions(int partitionCount)
        {
            if (partitionCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(partitionCount));
            }
            IEnumerator<TSource>[] partitions
                = new IEnumerator<TSource>[partitionCount];
            IEnumerable<KeyValuePair<long, TSource>> partitionEnumerable = Partitioner.Create(_data, true).GetOrderableDynamicPartitions();
            for (int i = 0; i < partitionCount; i++)
            {
                partitions[i] = DropIndices(partitionEnumerable.GetEnumerator());
            }
            return partitions;
        }

        public override IEnumerable<TSource> GetDynamicPartitions()
        {
            return DropIndices(Partitioner.Create(_data, true).GetOrderableDynamicPartitions());
        }

        private static IEnumerable<TSource> DropIndices(IEnumerable<KeyValuePair<long, TSource>> source)
        {
            foreach (KeyValuePair<long, TSource> pair in source)
            {
                yield return pair.Value;
            }
        }

        private static IEnumerator<TSource> DropIndices(IEnumerator<KeyValuePair<long, TSource>> source)
        {
            while (source.MoveNext())
            {
                yield return source.Current.Value;
            }
        }

        public override bool SupportsDynamicPartitions
        {
            get { return true; }
        }
    }

}