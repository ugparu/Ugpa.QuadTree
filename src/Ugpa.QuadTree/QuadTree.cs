using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ugpa.QuadTree
{
    public sealed partial class QuadTree<T> : IEnumerable<T>
    {
        private readonly Dictionary<(int, int), QuadTreeNode> nodes = new();

        private readonly int nodeSize;
        private readonly int maxDepth;

        public QuadTree(int nodeSize)
            : this(nodeSize, 4)
        {
        }

        public QuadTree(int nodeSize, int maxDepth)
        {
            if (nodeSize < 1)
                throw new ArgumentException(nameof(nodeSize));

            if ((nodeSize & (nodeSize - 1)) != 0) // not power of 2
                throw new ArgumentException(nameof(nodeSize));

            this.maxDepth = maxDepth;
            this.nodeSize = nodeSize;
        }

        public void Put(T item, int left, int top, int right, int bottom)
        {
            if (left > right || bottom > top)
                throw new ArgumentException();

            for (int i = left / nodeSize; i <= right / nodeSize; i++)
            {
                for (int j = bottom / nodeSize; j <= top / nodeSize; j++)
                {
                    if (!nodes.TryGetValue((i, j), out var node))
                    {
                        node = new QuadTreeNode(i * nodeSize, j * nodeSize, nodeSize);
                        nodes.Add((i, j), node);
                    }

                    node.Put(item, left, top, right, bottom, maxDepth - 1);
                }
            }
        }

        public IEnumerable<T> Pick(int x, int y)
        {
            var i = x / nodeSize;
            var j = y / nodeSize;

            return nodes.TryGetValue((i, j), out var node)
                ? node.Pick(x, y)
                : Enumerable.Empty<T>();
        }

        public IEnumerable<T> Pick(int left, int top, int right, int bottom)
        {
            if (left > right || bottom > top)
                throw new ArgumentException();

            for (int i = left / nodeSize; i <= right / nodeSize; i++)
            {
                for (int j = bottom / nodeSize; j <= top / nodeSize; j++)
                {
                    if (nodes.TryGetValue((i, j), out var node))
                    {
                        foreach (var item in node.Pick(left, top, right, bottom))
                        {
                            yield return item;
                        }
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
            => nodes.SelectMany(_ => _.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
