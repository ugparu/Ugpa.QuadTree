using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ugpa.QuadTree
{
    /// <summary>
    /// Provides quad tree functionality.
    /// </summary>
    /// <typeparam name="T">Type of element.</typeparam>
    public sealed partial class QuadTree<T> : IEnumerable<T>
    {
        private readonly Dictionary<(int, int), QuadTreeNode> nodes = new();

        private readonly int nodeSize;
        private readonly int maxDepth;

        /// <summary>
        /// Initialize new instance of <see cref="QuadTree{T}"/> with max depth equals to 4.
        /// </summary>
        /// <param name="nodeSize"><inheritdoc cref="QuadTree{T}.QuadTree(int, int)" path="/param[@name='nodeSize']"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Node size parameter value is less than 1 or not is power of 2.</exception>
        public QuadTree(int nodeSize)
            : this(nodeSize, 4)
        {
        }

        /// <summary>
        /// Initialize new instance of <see cref="QuadTree{T}"/>.
        /// </summary>
        /// <param name="nodeSize">Node size on the top layer of the tree.</param>
        /// <param name="maxDepth">Max depth of the tree.</param>
        /// <exception cref="ArgumentOutOfRangeException">Node size parameter value is less than 1 or not is power of 2.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Max depth parameter value is less than 1.</exception>
        public QuadTree(int nodeSize, int maxDepth)
        {
            if (nodeSize < 1 || (nodeSize & (nodeSize - 1)) != 0) // not power of 2
                throw new ArgumentOutOfRangeException(nameof(nodeSize));

            if (maxDepth < 1)
                throw new ArgumentOutOfRangeException(nameof(maxDepth));

            this.maxDepth = maxDepth;
            this.nodeSize = nodeSize;
        }

        /// <summary>
        /// Puts an item with specified bounds inside tree.
        /// </summary>
        /// <param name="item">Item to put in tree.</param>
        /// <param name="left">Left bound of the item.</param>
        /// <param name="top">Top bound of the item.</param>
        /// <param name="right">Right bound of the item.</param>
        /// <param name="bottom">Bottom bound of the item.</param>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Picks all items in specifed location.
        /// </summary>
        /// <param name="x">X componen of location.</param>
        /// <param name="y">Y component of location/</param>
        /// <returns>All items in cpecified location.</returns>
        public IEnumerable<T> Pick(int x, int y)
        {
            var i = x / nodeSize;
            var j = y / nodeSize;

            return nodes.TryGetValue((i, j), out var node)
                ? node.Pick(x, y)
                : Enumerable.Empty<T>();
        }

        /// <summary>
        /// Picks all items in specifed area.
        /// </summary>
        /// <param name="left">Left bound of the area.</param>
        /// <param name="top">Top bound of the area.</param>
        /// <param name="right">Right bound of the area.</param>
        /// <param name="bottom">Bottom bound of the area.</param>
        /// <returns>All items in specifed area.</returns>
        /// <exception cref="ArgumentException">Left bound is greater than right bound.</exception>
        /// <exception cref="ArgumentException">Bottom bound is greater than top bound.</exception>
        /// <remarks>Because of one item can be placed in multiple quadrants, duplicate items can be returned.</remarks>
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

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
            => nodes.SelectMany(_ => _.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
