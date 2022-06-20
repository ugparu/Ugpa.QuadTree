using System.Collections.Generic;
using System.Linq;

namespace Ugpa.QuadTree.Benchmark
{
    public class SimplePointInBoundsBenchmark : PointHitTestBenchmark
    {
        protected override IEnumerable<BenchmarkData.Bounds> PointHitTest(int x, int y)
            => Items.Where(_ => _.Left <= x && _.Right >= x && _.Bottom <= y && _.Top >= y);
    }
}
