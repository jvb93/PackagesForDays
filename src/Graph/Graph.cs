using System;
using System.Collections.Generic;
using System.Linq;

// generic graph class heavily inspired by microsoft's Scott Mitchell
// https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
// uses modified DFS to find circular references

namespace Graph
{
    public class Graph<T>
    {
        private List<Node<T>> graphSet { get; set; }
        public List<Node<T>> Nodes {
            get{ return this.graphSet;}

        }
        public int Count => graphSet.Count;

        public Graph()
        {
            graphSet = new List<Node<T>>();
        }

        public Node<T> AddNode(Node<T> node)
        {
            graphSet.Add(node);
            return node;
        }

        public Node<T> AddNode(T value)
        {
           return AddNode(new Node<T>(value));
        }

        public void AddDirectedEdge(Node<T> from, Node<T> to)
        {
            from.Children.Add(to);
        }
        

        public bool Contains(T value)
        {
            return graphSet.Find(node=>node.Value.Equals(value)) != null;
        }

        public Node<T> GetNodeWithValue(T value)
        {
            return graphSet.Find(node => node.Value.Equals(value));
        }

        public bool Remove(T value)
        {
            //remove the node
            Node<T> nodeToRemove = GetNodeWithValue(value);

            if (nodeToRemove == null)
            {
                // node wasn't found
                return false;
            }
                        
            graphSet.Remove(nodeToRemove);

            // remove each node that is a child of this node
            foreach (Node<T> node in graphSet)
            {
                int index = node.Children.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node
                    node.Children.RemoveAt(index);
                }
            }

            return true;
        }

        /// <summary>
        /// Topologically sort this graph
        /// this can also detect whether or not a circular reference is present by throwing an argument exception if true
        /// reference: https://en.wikipedia.org/wiki/Topological_sorting
        /// </summary>
        /// <returns>A non-unique representation of the sorted graph</returns>
        public string TopologicalSort()
        {
            var sorted = new List<Node<T>>();

            // keep a reference to each node and whether or not we have encountered it in the recursive steps 
            // if a given node is "true" that means there is a circular reference
            Dictionary<Node<T>, bool> callStack = new Dictionary<Node<T>, bool>();

            //loop through each node
            foreach (var node in Nodes)
            {
                // only bother visiting non-visited nodes
                if (!node.IsVisited)
                {
                    Visit(node, sorted, callStack);
                }
            }

            //cleanup the nodes by unvisiting
            UnvisitEachNode();

            //turn the sorted list into an array if possible
            return sorted.Any() ?  string.Join(", ", sorted.Select(x=>x.Value)) : null;
        }
        private void Visit(Node<T> node, List<Node<T>> sorted, Dictionary<Node<T>, bool> callStack)
        {

            // check if we have encountered this node before in a previous iteration of this function
            // if it's in our call stack and marked as true, that means this isn't the first time we've been here and a cycle is detected
            if (callStack.ContainsKey(node) && callStack[node])
            {
                
               throw new ArgumentException("The dependency tree definition contains a circular reference, this is invalid.");
             
            }

            //we've visited this node before, but not as a part of a recursive cycle, so we can start backtracking the recursion
            if (node.IsVisited)
            {
                return;
            }

            //never visited, so let's visit the node and add it to our callstack
            node.IsVisited = true;
            callStack.Add(node, true);

            //recursively repeat the above process for each child of this node          
            foreach (var child in node.Children)
            {
                Visit(child, sorted, callStack);
            }

            //we checked every child of this node and found no circular references
            callStack[node] = false;

            //add this node in the correct order to our sorted list
            sorted.Add(node);
        }

        private void UnvisitEachNode()
        {
            foreach (Node<T> node in Nodes)
            {
                node.IsVisited = false;
            }
        }

    }

}
