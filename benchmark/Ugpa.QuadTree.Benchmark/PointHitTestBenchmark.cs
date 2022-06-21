using System.Collections.Generic;
using System.Linq;

namespace Ugpa.QuadTree.Benchmark
{
    public abstract class PointHitTestBenchmark : BenchmarkBase
    {
        private BenchmarkData.Point hitTestPoint;

        public object PointHitTest()
            => PointHitTest(hitTestPoint.X, hitTestPoint.Y).ToArray();

        protected override void IterationSetup(int iterationIndex)
        {
            base.IterationSetup(iterationIndex);
            hitTestPoint = BenchmarkData.Instance.GetPoint(iterationIndex);
        }

        protected abstract IEnumerable<BenchmarkData.Bounds> PointHitTest(int x, int y);
    }
}
