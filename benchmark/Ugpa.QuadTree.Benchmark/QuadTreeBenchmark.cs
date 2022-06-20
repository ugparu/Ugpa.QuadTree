using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Ugpa.QuadTree.Benchmark
{
    public class QuadTreeBenchmark : BenchmarkBase
    {
        private QuadTree<BenchmarkData.Bounds> tree;

        [Params(1, 2, 3, 4, 5)]
        public int MaxDepth { get; set; }

        [Params(256, 512, 1024)]
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

        protected override IEnumerable<BenchmarkData.Bounds> BoundsHitTest(int left, int top, int right, int bottom)
            => tree.Pick(left, top, right, bottom).Distinct();
    }
}
