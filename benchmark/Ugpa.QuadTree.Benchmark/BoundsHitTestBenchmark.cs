using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Ugpa.QuadTree.Benchmark
{
    public abstract class BoundsHitTestBenchmark : BenchmarkBase
    {
        private BenchmarkData.Bounds hitTestBounds;

        [ParamsSource(nameof(AreaMaxSizes))]
        public int AreaMaxSize { get; set; }

        public int[] AreaMaxSizes => QuadTreeBenchmarkParams.AreaMaxSizes;

        [Benchmark]
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
