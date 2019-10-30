// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using MicroBenchmarks;

namespace System.Linq.Tests
{
    [BenchmarkCategory(Categories.CoreFX, Categories.LINQ)]
    public class Perf_ParallelEnumerable
    {
        private readonly Consumer _consumer = new Consumer();

        // used by benchmarks that have no special handling per collection type
        public IEnumerable<object> DefaultArgument()
        {
            yield return LinqTestData.IEnumerable.Collection.AsParallel();
        }

        // used by benchmarks that have special handling with ordered collections
        public IEnumerable<object> OrderArguments()
        {
            yield return LinqTestData.IEnumerable.Collection.AsParallel();
            yield return LinqTestData.IOrderedEnumerable.Collection.AsParallel();
            yield return LinqTestData.IOrderedEnumerable.Collection.AsParallel().AsOrdered();
        }

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void AsParallel(ParallelQuery<int> query) => query.Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void AsOrdered(ParallelQuery<int> query) => query.Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void Select(ParallelQuery<int> query) => query.Select(i => i + 1).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void Where(ParallelQuery<int> query) => query.Where(i => i >= 0).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public int FirstWithPredicate_LastElementMatches(ParallelQuery<int> query) => query.First(i => i >= LinqTestData.Size - 1);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public int LastWithPredicate_FirstElementMatches(ParallelQuery<int> query) => query.Last(i => i >= 0);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public bool WhereAny_LastElementMatches(ParallelQuery<int> query) => query.Where(i => i >= LinqTestData.Size - 1).Any();

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public bool AnyWithPredicate_LastElementMatches(ParallelQuery<int> query) => query.Any(i => i >= LinqTestData.Size - 1);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public bool All_AllElementsMatch(ParallelQuery<int> query) => query.All(i => i >= 0);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public int SingleWithPredicate_LastElementMatches(ParallelQuery<int> query) => query.Single(i => i >= LinqTestData.Size - 1);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public int SingleWithPredicate_FirstElementMatches(ParallelQuery<int> query) => query.Single(i => i <= 0);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void CastToBaseClass(ParallelQuery<int> query) => query.Cast<object>().Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void CastToSameType(ParallelQuery<int> query) => query.Cast<int>().Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void OrderBy(ParallelQuery<int> query) => query.OrderBy(i => i).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void OrderByDescending(ParallelQuery<int> query) => query.OrderByDescending(i => i).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void OrderByThenBy(ParallelQuery<int> query) => query.OrderBy(i => i).ThenBy(i => -i).Consume(_consumer);

        [Benchmark]
        public void Range() => ParallelEnumerable.Range(0, LinqTestData.Size).Consume(_consumer);

        [Benchmark]
        public void Repeat() => ParallelEnumerable.Repeat(0, LinqTestData.Size).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void Reverse(ParallelQuery<int> query) => query.Reverse().Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void Skip_One(ParallelQuery<int> query) => query.Skip(1).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void Take_All(ParallelQuery<int> query) => query.Take(LinqTestData.Size - 1).Consume(_consumer);

#if !NETFRAMEWORK
        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void TakeLastHalf(ParallelQuery<int> query) => query.TakeLast(LinqTestData.Size / 2).Consume(_consumer);
#endif

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void SkipHalfTakeHalf(ParallelQuery<int> query) => query.Skip(LinqTestData.Size / 2).Take(LinqTestData.Size / 2).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public int[] ToArray(ParallelQuery<int> query) => query.ToArray();

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public int[] SelectToArray(ParallelQuery<int> query) => query.Select(i => i + 1).ToArray();

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public List<int> ToList(ParallelQuery<int> query) => query.ToList();

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public List<int> SelectToList(ParallelQuery<int> query) => query.Select(i => i + 1).ToList();

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public Dictionary<int, int> ToDictionary(ParallelQuery<int> query) => query.ToDictionary(key => key);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public bool Contains_ElementNotFound(ParallelQuery<int> query) => query.Contains(LinqTestData.Size + 1);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void Concat_Once(ParallelQuery<int> query) => query.Concat(query).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public int Sum(ParallelQuery<int> query) => query.Sum();

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public int Min(ParallelQuery<int> query) => query.Min();

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public int Max(ParallelQuery<int> query) => query.Max();

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public double Average(ParallelQuery<int> query) => query.Average();

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public int Count(ParallelQuery<int> query) => query.Count();

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public double Aggregate(ParallelQuery<int> query) => query.Aggregate((x, y) => x + y);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public double Aggregate_Seed(ParallelQuery<int> query) => query.Aggregate(0, (x, y) => x + y);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void Distinct(ParallelQuery<int> query) => query.Distinct().Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public int ElementAt(ParallelQuery<int> query) => query.ElementAt(LinqTestData.Size / 2);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void GroupBy(ParallelQuery<int> query) => query.GroupBy(x => x % 10).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void GroupBy_ElementSelector(ParallelQuery<int> query) => query.GroupBy(x => x % 10, y => y).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(DefaultArgument))]
        public void Zip(ParallelQuery<int> query) => query.Zip(query, (x, y) => x + y).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void Intersect(ParallelQuery<int> query) => query.Intersect(query).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(OrderArguments))]
        public void Except(ParallelQuery<int> query) => query.Except(query).Consume(_consumer);

        [Benchmark]
        public void EmptyTakeSelectToArray() => ParallelEnumerable.Empty<int>().Take(10).Select(i => i).ToArray();
    }
}