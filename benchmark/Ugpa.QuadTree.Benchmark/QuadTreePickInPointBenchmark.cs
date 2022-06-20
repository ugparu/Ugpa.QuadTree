using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Ugpa.QuadTree.Benchmark
{
    public class QuadTreePickInPointBenchmark : PointHitTestBenchmark
    {
        private QuadTree<BenchmarkData.Bounds> tree;

        [ParamsSource(nameof(MaxDepths))]
        public int MaxDepth { get; set; }

        [ParamsSource(nameof(NodeSizes))]
        public int NodeSize { get; set; }

        public int[] MaxDepths => QuadTreeBenchmarkParams.MaxDepths;

        public int[] NodeSizes => QuadTreeBenchmarkParams.NodeSizes;

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
