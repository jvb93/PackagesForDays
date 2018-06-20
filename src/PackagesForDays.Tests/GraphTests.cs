
using System.Linq;
using Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PackagesForDays.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void CanAddNodeWithNoChildren()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddNode(5);

            Assert.AreEqual(graph.Nodes.Count, 1);

        }

        [TestMethod]
        public void DoesCountWork()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddNode(5);

            Assert.AreEqual(graph.Count, 1);

        }

        [TestMethod]
        public void CanAddNodesAsDirectedChild()
        {
            Graph<int> graph = new Graph<int>();
            var parent = graph.AddNode(5);
            var child = graph.AddNode(3);

            graph.AddDirectedEdge(parent, child);

            Assert.AreEqual(graph.Count, 2);
            Assert.AreEqual(parent.Children.Count, 1);
            Assert.AreEqual(parent.Children.First().Value, child.Value );

        }

        [TestMethod]
        public void CanGetNodeWithValue()
        {
            Graph<int> graph = new Graph<int>();
            var added = graph.AddNode(5);
            var found = graph.GetNodeWithValue(5);
            
            Assert.AreEqual(added.Value, found.Value);
         

        }

        [TestMethod]
        public void DoesGraphContainsWork()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddNode(5);
            
            Assert.IsTrue(graph.Contains(5));


        }



    }
}
