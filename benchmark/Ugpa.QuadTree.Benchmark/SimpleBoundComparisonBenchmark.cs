using System.Collections.Generic;
using System.Linq;

namespace Ugpa.QuadTree.Benchmark
{
    public class SimpleBoundComparisonBenchmark : BenchmarkBase
    {
        protected override IEnumerable<BenchmarkData.Bounds> PointHitTest(int x, int y)
            => Items.Where(_ => _.Left <= x && _.Right >= x && _.Bottom <= y && _.Top >= y);

        protected override IEnumerable<BenchmarkData.Bounds> BoundsHitTest(int left, int top, int right, int bottom)
            => Items.Where(_ => _.Left <= right && _.Right >= left && _.Bottom <= top && _.Top >= bottom);
    }
}
