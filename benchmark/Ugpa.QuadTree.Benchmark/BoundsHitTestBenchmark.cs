using System.Collections.Generic;
using System.Linq;

namespace Ugpa.QuadTree.Benchmark
{
    public abstract class BoundsHitTestBenchmark : BenchmarkBase
    {
        private BenchmarkData.Bounds hitTestBounds;

        public int AreaMaxSize { get; set; }

        public object BoundsHitTest()
            => BoundsHitTest(hitTestBounds.Left, hitTestBounds.Top, hitTestBounds.Right, hitTestBounds.Bottom).ToArray();

        protected override void IterationSetup(int iterationIndex)
        {
            base.IterationSetup(iterationIndex);
            hitTestBounds = BenchmarkData.Instance.GetBounds(AreaMaxSize, iterationIndex);
        }

        protected abstract IEnumerable<BenchmarkData.Bounds> BoundsHitTest(int left, int top, int right, int bottom);
    }
}
