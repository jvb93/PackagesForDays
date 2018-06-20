
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

        [TestMethod]
        public void DoesGraphContainCircularReference()
        {
            Graph<int> circular = new Graph<int>();
            var circularParent = circular.AddNode(5);
            var circularChild = circular.AddNode(3);

            circular.AddDirectedEdge(circularParent, circularChild);
            circular.AddDirectedEdge(circularChild, circularParent);

            Assert.IsTrue(circular.ContainsCircularReference());


        }

        [TestMethod]
        public void DoesGraphNotContainCircularReference()
        {
            Graph<int> graph = new Graph<int>();
            var parent = graph.AddNode(5);
            var child = graph.AddNode(3);
            var grandchild = graph.AddNode(4);
            
            graph.AddDirectedEdge(parent, child);
            graph.AddDirectedEdge(child, grandchild);

            Assert.IsFalse(graph.ContainsCircularReference());

            var emptyGraph = new Graph<int>();
            Assert.IsFalse(emptyGraph.ContainsCircularReference());

            var simpleGraph = new Graph<int>();
            simpleGraph.AddNode(5);
            Assert.IsFalse(simpleGraph.ContainsCircularReference());



        }

        [TestMethod]
        public void CanTopologicSort()
        {
            Graph<int> graph = new Graph<int>();
            var parent = graph.AddNode(5);
            var child = graph.AddNode(3);
            var grandchild = graph.AddNode(4);

            graph.AddDirectedEdge(parent, child);
            graph.AddDirectedEdge(child, grandchild);

            var randomNode = graph.AddNode(50);

            var sorted = graph.TopologicalSort();
            Assert.IsFalse(string.IsNullOrWhiteSpace(sorted));

        }



    }
}
