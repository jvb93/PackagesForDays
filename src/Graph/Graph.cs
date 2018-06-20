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
            Node<T> nodeToRemove = graphSet.Find(node=>node.Value.Equals(value));

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
        /// Check to see if this graph contains any circular references
        /// </summary>
        /// <returns>True if there is a cycle in the graph</returns>
        public bool ContainsCircularReference()
        {
            //keep a reference to each node and whether or not we have encountered it in the recursive steps 
            // if a given node is "true" that means there is a circular reference
            Dictionary<Node<T>, bool> callStack = new Dictionary<Node<T>, bool>();

            //loop through each node in the graph
            foreach(var node in Nodes)
            { 
                // if the reference finder finds a circular reference from this node, we can short circuit
                // simply returning true
                if (CircularReferenceFinder(node, callStack))
                {
                    //cleaup our visited nodes
                    UnvisitEachNode();
                    return true;
                }
            }

            //we went through each node and found no circular references therefore there are no cycles in this graph
            //cleaup our visited nodes
            UnvisitEachNode();
            return false;
        }

        /// <summary>
        /// Recursively look at each node and see if there are any circular references using modified DFS
        /// </summary>
        /// <param name="parent">the root node</param>
        /// <param name="callStack"> a reference to each node previously encountered</param>
        /// <returns></returns>
        private bool CircularReferenceFinder(Node<T> parent, Dictionary<Node<T>, bool> callStack)
        {
            // check if we have encountered this node before in a previous iteration of this function
            // if it's in our call stack and marked as true, that means this isn't the first time we've been here and a cycle is detected
            if (callStack.ContainsKey(parent) && callStack[parent])
            {
                return true;

            }
            //we've visited this node before, but not as a part of a recursive cycle, so we can return false (no cycles) here
            if (parent.IsVisited)
            {
                return false;
            }

            //this is a newly visited node, so we mark it as visited and add it to our callstack 
            parent.IsVisited = true;       
            callStack.Add(parent, true);

            //recursively repeat the above process for each child of this node          
            foreach (var node in parent.Children)
            {
                if (CircularReferenceFinder(node, callStack))
                {
                    //we found a circular reference somewhere down the line...
                    return true;
                }
                   
            }
        
            //we checked every child of this node and found no circular references
            callStack[parent] = false;

            return false;
        }

        /// <summary>
        /// Topologically sort this graph
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
