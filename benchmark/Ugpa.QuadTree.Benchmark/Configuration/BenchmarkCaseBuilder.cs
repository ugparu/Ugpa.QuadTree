using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Ugpa.QuadTree.Benchmark.Configuration
{
    internal abstract class BenchmarkCaseBuilder
    {
        public abstract BenchmarkCase[] Build(ImmutableConfig config);
    }
}
