using System.Collections.Generic;
using System.Linq;

namespace Ugpa.QuadTree.Benchmark
{
    public class SimpleBoundsIntersectionBenchmark : BoundsHitTestBenchmark
    {
        protected override IEnumerable<BenchmarkData.Bounds> BoundsHitTest(int left, int top, int right, int bottom)
            => Items.Where(_ => _.Left <= right && _.Right >= left && _.Bottom <= top && _.Top >= bottom);
    }
}
