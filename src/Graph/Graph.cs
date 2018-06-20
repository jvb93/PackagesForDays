using System;
using System.Collections.Generic;

// generic graph class heavily inspired by microsoft's Scott Mitchell
// https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx

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



    }

}
