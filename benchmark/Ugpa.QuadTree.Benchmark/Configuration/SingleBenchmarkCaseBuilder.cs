using System.Collections.Generic;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Parameters;
using BenchmarkDotNet.Running;

namespace Ugpa.QuadTree.Benchmark.Configuration
{
    internal sealed class SingleBenchmarkCaseBuilder<T> : BenchmarkCaseBuilder<T, SingleBenchmarkCaseBuilder<T>>
    {
        private SingleBenchmarkCaseBuilder(Job job)
            : base(job)
        {
        }

        public static SingleBenchmarkCaseBuilder<T> Create(Job job)
        {
            return new SingleBenchmarkCaseBuilder<T>(job);
        }

        public override BenchmarkCase[] Build(ImmutableConfig config)
        {
            var caseDescription = new Descriptor(
                typeof(T),
                Workload,
                globalSetupMethod: GlobalSetup,
                globalCleanupMethod: null,
                iterationSetupMethod: IterationSetup,
                iterationCleanupMethod: null,
                description: null,
                additionalLogic: null,
                baseline: false,
                categories: null,
                operationsPerInvoke: 1,
                methodIndex: 0);

            return new[] { BenchmarkCase.Create(caseDescription, Job, new ParameterInstances(new List<ParameterInstance>()), config) };
        }
    }
}
