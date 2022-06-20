using System.Collections;
using System.Collections.Generic;

namespace Ugpa.QuadTree
{
    partial class QuadTree<T>
    {
        private sealed class QuadTreeNode : IEnumerable<T>
        {
            private const int NE = 0;
            private const int NW = 1;
            private const int SW = 2;
            private const int SE = 3;

            private readonly QuadTreeNode[] children = new QuadTreeNode[4];

            private readonly int x;
            private readonly int y;
            private readonly int size;

            private ICollection<(T item, int left, int top, int right, int bottom)>? items;

            public QuadTreeNode(int x, int y, int size)
            {
                this.x = x;
                this.y = y;
                this.size = size;
            }

            public void Put(T item, int left, int top, int right, int bottom, int depth)
            {
                if (size == 1 ||
                    depth == 0 ||
                    (
                        left <= x &&
                        bottom <= y &&
                        right >= (x + size - 1) &&
                        top >= (y + size - 1)
                    ))
                {
                    (items ??= new LinkedList<(T, int, int, int, int)>()).Add((item, left, top, right, bottom));
                }
                else
                {
                    var childNodeSize = size >> 1;
                    var ax = x + childNodeSize;
                    var ay = y + childNodeSize;

                    if (top >= ay) // N
                    {
                        if (right >= ax) // NE
                        {
                            (children[NE] ??= new QuadTreeNode(ax, ay, childNodeSize))
                                .Put(item, left, top, right, bottom, depth - 1);
                        }

                        if (left < ax) // NW
                        {
                            (children[NW] ??= new QuadTreeNode(x, ay, childNodeSize))
                                .Put(item, left, top, right, bottom, depth - 1);
                        }
                    }

                    if (bottom < ay) // S
                    {
                        if (left < ax) // SW
                        {
                            (children[SW] ??= new QuadTreeNode(x, y, childNodeSize))
                                .Put(item, left, top, right, bottom, depth - 1);
                        }

                        if (right >= ax) // SE
                        {
                            (children[SE] ??= new QuadTreeNode(ax, y, childNodeSize))
                                .Put(item, left, top, right, bottom, depth - 1);
                        }
                    }
                }
            }

            public IEnumerable<T> Pick(int x, int y)
            {
                if (items is not null)
                {
                    foreach (var item in items)
                    {
                        if (item.left <= x &&
                            item.right >= x &&
                            item.top >= y &&
                            item.bottom <= y)
                        {
                            yield return item.item;
                        }
                    }
                }

                foreach (var child in children)
                {
                    if (child is not null &&
                        child.x <= x &&
                        child.y <= y &&
                        child.x + child.size - 1 >= x &&
                        child.y + child.size - 1 >= y)
                    {
                        foreach (var item in child.Pick(x, y))
                            yield return item;
                    }
                }
            }

            public IEnumerable<T> Pick(int left, int top, int right, int bottom)
            {
                if (items is not null)
                {
                    foreach (var item in items)
                    {
                        if (item.left <= right &&
                            item.right >= left &&
                            item.top >= bottom &&
                            item.bottom <= top)
                        {
                            yield return item.item;
                        }
                    }
                }

                foreach (var child in children)
                {
                    if (child is not null &&
                        child.x <= right &&
                        child.y <= top &&
                        child.x + child.size - 1 >= left &&
                        child.y + child.size - 1 >= bottom)
                    {
                        foreach (var item in child.Pick(left, top, right, bottom))
                            yield return item;
                    }
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (items is not null)
                {
                    foreach (var item in items)
                    {
                        yield return item.item;
                    }
                }

                foreach (var child in children)
                {
                    if (child is not null)
                    {
                        foreach (var item in child)
                            yield return item;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}
