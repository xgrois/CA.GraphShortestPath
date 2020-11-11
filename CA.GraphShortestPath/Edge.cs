using System;
using System.Collections.Generic;
using System.Text;

namespace CA.GraphShortestPath
{
    class Edge<TNode>
    {
        public Node<TNode> Src { get; set; }
        public Node<TNode> Dst { get; set; }

        public int Weight { get; set; }

        public Edge(Node<TNode> src, Node<TNode> dst, int weight)
        {
            Src = src;
            Dst = dst;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"Edge ({Src.GraphIndex}) --[{Weight}]--> ({Dst.GraphIndex})";
        }
    }
}
