using System;
using System.Collections.Generic;
using System.Text;

namespace Graph
{
    public class Node<T>
    {

        public T Value { get; set; }
        public List<Node<T>> Children { get; set; }
        public bool IsVisited { get; set; }
        
        public Node(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("The value of a new node cannot be null.");
            }

            Value = value;
            Children = new List<Node<T>>();
        }

    }
}
