using System.Collections.Generic;

namespace Ugpa.QuadTree.Benchmark
{
    public abstract class BenchmarkBase
    {
        private int iterationIndex;

        protected IEnumerable<BenchmarkData.Bounds> Items { get; private set; }

        public virtual void GlobalSetup()
        {
            iterationIndex = -1;
            Items = BenchmarkData.Instance.Items;
        }

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
