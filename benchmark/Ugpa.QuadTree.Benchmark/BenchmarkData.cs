using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Ugpa.QuadTree.Benchmark
{
    public class BenchmarkData
    {
        private static Lazy<BenchmarkData> instance = new Lazy<BenchmarkData>(CreateInstance);

        private readonly Random rnd = new Random();

        private readonly List<Bounds> items = new List<Bounds>();
        private readonly ConcurrentDictionary<int, Point> points = new ConcurrentDictionary<int, Point>();
        private readonly ConcurrentDictionary<(int, int), Bounds> bounds = new ConcurrentDictionary<(int, int), Bounds>();

        private int left;
        private int top;
        private int right;
        private int bottom;

        public static BenchmarkData Instance => instance.Value;

        public IEnumerable<Bounds> Items => items;

        public Point GetPoint(int index)
        {
            return points.GetOrAdd(
                index,
                _ => new Point { X = rnd.Next(left, right + 1), Y = rnd.Next(bottom, top + 1) });
        }

        public Bounds GetBounds(int areaMaxSize, int index)
        {
            return bounds.GetOrAdd(
                (areaMaxSize, index),
                _ =>
                {
                    var width = rnd.Next(16, areaMaxSize);
                    var height = rnd.Next(16, areaMaxSize);
                    var left = rnd.Next(right - width);
                    var bottom = rnd.Next(top - height);
                    return new Bounds { Left = left, Top = bottom + height, Right = left + width, Bottom = bottom };
                });
        }

        private static BenchmarkData CreateInstance()
        {
            var data = new BenchmarkData();
            for (int i = 0; i < 10000; i++)
            {
                var width = data.rnd.Next(50, 300);
                var height = data.rnd.Next(20, 80);
                var left = data.rnd.Next(2048 - width);
                var bottom = data.rnd.Next(2048 - height);
                var right = left + width;
                var top = bottom + height;
                data.items.Add(new Bounds { Left = left, Top = top, Right = right, Bottom = bottom });
            }

            data.left = data.Items.Min(_ => _.Left);
            data.top = data.Items.Max(_ => _.Top);
            data.right = data.Items.Max(_ => _.Right);
            data.bottom = data.Items.Min(_ => _.Bottom);

            return data;
        }

        public sealed class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public sealed class Bounds
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
    }
}
