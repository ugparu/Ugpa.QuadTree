using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Ugpa.QuadTree.Benchmark
{
    public abstract class BenchmarkBase
    {
        private int iterationIndex;

        private BenchmarkData.Point hitTestPoint;
        private BenchmarkData.Bounds hitTestBounds;

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
            hitTestPoint = BenchmarkData.Instance.GetPoint(iterationIndex);
            hitTestBounds = BenchmarkData.Instance.GetBounds(iterationIndex);
        }

        [Benchmark]
        public object PointHitTest()
            => PointHitTest(hitTestPoint.X, hitTestPoint.Y).ToArray();

        [Benchmark]
        public object BoundsHitTest()
            => BoundsHitTest(hitTestBounds.Left, hitTestBounds.Top, hitTestBounds.Right, hitTestBounds.Bottom).ToArray();

        protected abstract IEnumerable<BenchmarkData.Bounds> PointHitTest(int x, int y);

        protected abstract IEnumerable<BenchmarkData.Bounds> BoundsHitTest(int left, int top, int right, int bottom);
    }
}
