using System.Collections.Generic;
using System.Linq;

namespace Ugpa.QuadTree.Benchmark
{
    public class QuadTreePickInPointBenchmark : PointHitTestBenchmark
    {
        private QuadTree<BenchmarkData.Bounds> tree;

        public int MaxDepth { get; set; }

        public int NodeSize { get; set; }

        public override void GlobalSetup()
        {
            base.GlobalSetup();
            tree = new QuadTree<BenchmarkData.Bounds>(NodeSize, MaxDepth);
            foreach (var item in Items)
                tree.Put(item, item.Left, item.Top, item.Right, item.Bottom);
        }

        protected override IEnumerable<BenchmarkData.Bounds> PointHitTest(int x, int y)
            => tree.Pick(x, y).Distinct();
    }
}
