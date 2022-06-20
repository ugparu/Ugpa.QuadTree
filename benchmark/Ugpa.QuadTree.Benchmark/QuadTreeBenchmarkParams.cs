namespace Ugpa.QuadTree.Benchmark
{
    internal static class QuadTreeBenchmarkParams
    {
        public static int[] MaxDepths => new[] { 1, 2, 3, 4, 5 };

        public static int[] NodeSizes => new[] { 256, 512, 1024 };

        public static int[] AreaMaxSizes => new[] { 128, 512, 2048 };
    }
}
