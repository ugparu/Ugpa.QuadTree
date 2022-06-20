using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Ugpa.QuadTree.Benchmark
{
    public abstract class BenchmarkBase
    {
        private int iterationIndex;

        protected IEnumerable<BenchmarkData.Bounds> Items { get; private set; }

        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            iterationIndex = -1;
            Items = BenchmarkData.Instance.Items;
        }

        [IterationSetup]
        public void IterationSetup()
        {
            iterationIndex++;
            IterationSetup(iterationIndex);
        }

        protected virtual void IterationSetup(int iterationIndex)
        {
        }
    }
}
