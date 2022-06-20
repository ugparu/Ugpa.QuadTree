using System.Linq;
using Xunit;

namespace Ugpa.QuadTree.Tests
{
    public class QuadTreeTest
    {
        [Fact]
        public void EmptyTreePickTest()
        {
            var tree = new QuadTree<object>(16);
            var pick = tree.Pick(8, 8);
            Assert.Empty(pick);
        }

        [Fact]
        public void EmptyTreeEnumerateTest()
        {
            var tree = new QuadTree<object>(16);
            var items = tree.ToArray();
            Assert.Empty(items);
        }

        [Theory]
        [InlineData(0, 1, 1, 0, 1)]
        [InlineData(0, 3, 1, 2, 1)]
        [InlineData(2, 3, 3, 2, 1)]
        [InlineData(2, 1, 3, 0, 1)]
        [InlineData(0, 1, 3, 0, 2)]
        [InlineData(0, 3, 3, 2, 2)]
        [InlineData(0, 3, 1, 0, 2)]
        [InlineData(2, 3, 3, 0, 2)]
        [InlineData(1, 3, 3, 2, 3)]
        [InlineData(0, 2, 1, 1, 4)]
        [InlineData(1, 3, 2, 2, 4)]
        [InlineData(2, 2, 3, 1, 4)]
        [InlineData(1, 1, 2, 0, 4)]
        [InlineData(1, 2, 2, 1, 4)]
        public void SingleItemEnumerateTest(int left, int top, int right, int bottom, int expectedEntryCount)
        {
            var tree = new QuadTree<object>(4);
            var item = new object();
            tree.Put(item, left, top, right, bottom);
            var items = tree.ToArray();
            Assert.Equal(expectedEntryCount, items.Length);
            Assert.All(items, _ => Assert.Same(item, _));
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(0, 0, 1, 1)]
        [InlineData(1, 1, 1, 1)]
        [InlineData(1, 1, 1, 2)]
        [InlineData(1, 1, 2, 1)]
        [InlineData(1, 1, 2, 2)]
        public void PickSingleItemTest(int x, int y, int pickX, int pickY)
        {
            var tree = new QuadTree<object>(4);
            var item = new object();
            tree.Put(item, x, y + 1, x + 1, y);
            var pickItems = tree.Pick(pickX, pickY);
            var pick = Assert.Single(pickItems);
            Assert.Same(item, pick);
        }

        [Fact]
        public void PickMutipleItemsTest()
        {
            var tree = new QuadTree<object>(4);
            var item1 = new object();
            var item2 = new object();
            tree.Put(item1, 2, 3, 3, 2);
            tree.Put(item2, 1, 2, 2, 1);

            var pickItems = tree.Pick(2, 2).ToArray();
            Assert.Equal(2, pickItems.Length);
            Assert.Contains(item1, pickItems);
            Assert.Contains(item2, pickItems);
        }

        [Theory]
        [InlineData(1, 2, 2, 1)]
        [InlineData(0, 3, 3, 0)]
        public void PickItemsInRegionTest(int pickLeft, int pickTop, int pickRight, int pickBottom)
        {
            var tree = new QuadTree<object>(4);
            var item1 = new object();
            var item2 = new object();
            var item3 = new object();
            var item4 = new object();
            tree.Put(item1, 2, 3, 3, 2);
            tree.Put(item2, 0, 3, 1, 2);
            tree.Put(item3, 0, 1, 1, 0);
            tree.Put(item4, 2, 1, 3, 0);

            var pickItems = tree.Pick(pickLeft, pickTop, pickRight, pickBottom).ToArray();
            Assert.Equal(4, pickItems.Length);
            Assert.Contains(item1, pickItems);
            Assert.Contains(item2, pickItems);
            Assert.Contains(item3, pickItems);
            Assert.Contains(item4, pickItems);
        }

        [Fact]
        public void PickItemsInPartialRegionTest()
        {
            var tree = new QuadTree<object>(4);
            var item1 = new object();
            var item2 = new object();
            var item3 = new object();
            var item4 = new object();
            tree.Put(item1, 0, 1, 2, 0);
            tree.Put(item2, 0, 3, 2, 1);
            tree.Put(item3, 2, 2, 3, 1);
            tree.Put(item4, 0, 3, 0, 0);

            var pickItems = tree.Pick(1, 1, 2, 0).ToArray();
            var pickDistinctItems = pickItems.Distinct().ToArray();

            Assert.Equal(6, pickItems.Length);
            Assert.Equal(3, pickDistinctItems.Length);

            Assert.Contains(item1, pickItems);
            Assert.Contains(item2, pickItems);
            Assert.Contains(item3, pickItems);
            Assert.DoesNotContain(item4, pickItems);
        }

        [Fact]
        public void PickItemsMaxDepthTest()
        {
            var tree = new QuadTree<object>(64);
            var item1 = new object();
            var item2 = new object();
            tree.Put(item1, 0, 0, 0, 0);
            tree.Put(item2, 0, 1, 0, 1);

            var pickItems1 = tree.Pick(0, 0);
            var pick1 = Assert.Single(pickItems1);
            Assert.Same(item1, pick1);

            var pickItems2 = tree.Pick(0, 1);
            var pick2 = Assert.Single(pickItems2);
            Assert.Same(item2, pick2);
        }

        [Fact]
        public void PickItemsInRegionMaxDepthTest()
        {
            var tree = new QuadTree<object>(64);
            var item1 = new object();
            var item2 = new object();
            tree.Put(item1, 0, 0, 0, 0);
            tree.Put(item2, 0, 1, 0, 1);

            var pickItems1 = tree.Pick(0, 0, 0, 0);
            var pick1 = Assert.Single(pickItems1);
            Assert.Same(item1, pick1);

            var pickItems2 = tree.Pick(0, 1, 0, 1);
            var pick2 = Assert.Single(pickItems2);
            Assert.Same(item2, pick2);
        }
    }
}
