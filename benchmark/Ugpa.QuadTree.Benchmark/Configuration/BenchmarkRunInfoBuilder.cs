using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Ugpa.QuadTree.Benchmark.Configuration
{
    internal sealed class BenchmarkRunInfoBuilder
    {
        private List<BenchmarkCaseBuilder> cases = new List<BenchmarkCaseBuilder>();

        public static BenchmarkRunInfoBuilder Create()
        {
            return new BenchmarkRunInfoBuilder();
        }

        public BenchmarkRunInfo Build(ImmutableConfig config)
        {
            return new BenchmarkRunInfo(
                cases.SelectMany(_ => _.Build(config)).ToArray(),
                null,
                config);
        }

        public BenchmarkRunInfoBuilder AddCase<T>(Job job, Action<SingleBenchmarkCaseBuilder<T>> configure)
        {
            var caseBuilder = SingleBenchmarkCaseBuilder<T>.Create(job);
            configure(caseBuilder);
            cases.Add(caseBuilder);
            return this;
        }

        public BenchmarkRunInfoBuilder AddCases<T>(Job job, Action<BenchmarkCaseGroupBuilder<T>> configure)
        {
            var caseBuilder = BenchmarkCaseGroupBuilder<T>.Create(job);
            configure(caseBuilder);
            cases.Add(caseBuilder);
            return this;
        }
    }
}
