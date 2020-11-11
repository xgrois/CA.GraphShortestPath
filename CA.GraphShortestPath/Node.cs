using System;
using System.Collections.Generic;
using System.Text;

namespace CA.GraphShortestPath
{
    /// <summary>
    /// Node to be used in Graph class
    /// </summary>
    /// <typeparam name="TNode">Actual data in the node (e.g., int, string, other)</typeparam>
    class Node<TNode>
    {
        #region Fields and Properties
        public int GraphIndex { get; set; }
        public TNode Label { get; set; }
        public List<Node<TNode>> Neighbors { get; set; }
        public List<int> Weights { get; set; }
        #endregion
        public Node(TNode label)
        {
            Label = label;
            Neighbors = new List<Node<TNode>>();
            Weights = new List<int>();
        }
        public override string ToString()
        {
            return $"Node graph index: {GraphIndex}. " +
                $"Label: {Label}. " +
                $"#Neighbors: {Neighbors.Count}. " +
                $"Weights: [{string.Join(", ", Weights)}]";
        }

    }
}
