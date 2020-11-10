using System;
using System.Collections.Generic;
using System.Linq;

namespace CA.GraphShortestPath
{
    /// <summary>
    /// Node to be used in Graph class
    /// </summary>
    /// <typeparam name="TLabel">Actual data in the node (e.g., int, string, other)</typeparam>
    class Node<TLabel>
    {
        #region Fields and Properties
        public int GraphIndex { get; set; }
        public TLabel Label { get; set; }
        public List<Node<TLabel>> Neighbors { get; set; }
        public List<int> Weights { get; set; }
        #endregion
        public Node(TLabel label)
        {
            Label = label;
            Neighbors = new List<Node<TLabel>>();
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

    /// <summary>
    /// Directed and weighted graph
    /// </summary>
    /// <typeparam name="TLabel">Actual data in the node (e.g., int, string, other)</typeparam>
    class Graph<TLabel>
    {
        #region Fields and Properties
        private List<Node<TLabel>> Nodes { get; set; }
        #endregion

        #region Constructor(s)
        public Graph()
        {
            Nodes = new List<Node<TLabel>>();
        }
        #endregion

        #region Building Process
        public Node<TLabel> AddNode(TLabel label)
        {
            var node = new Node<TLabel>(label);
            Nodes.Add(node);
            UpdateIndexes();
            return node;
        }

        private void UpdateIndexes()
        {
            int ind = 0;
            foreach (var node in Nodes)
            {
                node.GraphIndex = ind++;
            }
        }

        public void Connect(Node<TLabel> n1, Node<TLabel> n2, int weight)
        {
            n1.Neighbors.Add(n2);
            n1.Weights.Add(weight);
        }
        #endregion

        #region Shortest Path
        public List<Node<TLabel>> ShortestPath(Node<TLabel> src, Node<TLabel> dst)
        {
            List<Node<TLabel>> srtPath = new List<Node<TLabel>>();

            (int[] distances, int[] previous) = Dijsktra(src);

            previous[src.GraphIndex] = -1;
            BuildPath(srtPath, dst.GraphIndex, previous);
            srtPath.Reverse();
            return srtPath;
        }

        private void BuildPath(List<Node<TLabel>> srtPath, int i, int[] previous)
        {
            if (previous[i] == -1)
            {
                srtPath.Add(Nodes[i]);
                return;
            }
            else
            {
                srtPath.Add(Nodes[i]);
                BuildPath(srtPath, previous[i], previous);
            }
        }

        public (int[], int[]) Dijsktra(Node<TLabel> src)
        {
            int N = Nodes.Count;
            bool[] visited = new bool[N];
            int[] distances = new int[N];
            int[] previous = new int[N];

            // Set all distances to "Inf" except src node to 0
            for (int i = 0; i < N; i++)
                distances[i] = int.MaxValue;

            distances[src.GraphIndex] = 0;

            Node<TLabel> node;
            int neighborLocalIndex, newDist;
            int index = src.GraphIndex;

            while (visited.Any(x => x == false)) // At least 1 node to visit
            {
                // Visit a node and update all neighborhood
                node = Nodes[index];
                visited[index] = true;
                foreach (var neighbor in node.Neighbors)
                {
                    if (!visited[neighbor.GraphIndex]) // Neighbor is not already visited
                    {
                        neighborLocalIndex = node.Neighbors.FindIndex(x => x == neighbor);
                        newDist = distances[node.GraphIndex] + node.Weights[neighborLocalIndex];
                        if (newDist < distances[neighbor.GraphIndex])
                        {
                            distances[neighbor.GraphIndex] = newDist;
                            previous[neighbor.GraphIndex] = node.GraphIndex;
                        }
                    }

                }
                // Find next unvisited node index at minimum distance
                index = PickNextNodeIndex(distances, visited);
            }

            return (distances, previous);

        }

        private int PickNextNodeIndex(int[] distances, bool[] visited)
        {
            int N = Nodes.Count;
            int currentMin = int.MaxValue;
            int currentMinIndex = 0;
            for (int i = 0; i < N; i++)
            {
                if (!visited[i])
                {
                    if (distances[i] < currentMin)
                    {
                        currentMin = distances[i];
                        currentMinIndex = i;
                    }
                }
            }
            return currentMinIndex;
        }
        #endregion

        #region Utils
        public override string ToString()
        {
            return $"Directed and weighted graph with {Nodes.Count} nodes.";
        }
        #endregion

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("::: SHORTEST PATH (Dijsktra) :::\n\r");

            // Build the graph
            var g = new Graph<string>();

            var n1 = g.AddNode("A");
            var n2 = g.AddNode("B");
            var n3 = g.AddNode("C");
            var n4 = g.AddNode("D");
            var n5 = g.AddNode("E");

            g.Connect(n1, n2, 10);
            g.Connect(n1, n4, 5);

            g.Connect(n2, n1, 1);
            g.Connect(n2, n3, 1);
            g.Connect(n2, n4, 2);

            g.Connect(n3, n2, 1);
            g.Connect(n3, n5, 4);

            g.Connect(n4, n2, 3);
            g.Connect(n4, n3, 9);
            g.Connect(n4, n5, 2);

            g.Connect(n5, n1, 15);
            g.Connect(n5, n3, 6);

            // Calculate the shortest path between two nodes
            var srtPath = g.ShortestPath(n5, n1);

            // Output the result in console
            Console.WriteLine($"Shortest Path from ({srtPath[0].Label}) to ({srtPath.Last().Label}): ");
            for (int i = 0; i < srtPath.Count - 1; i++)
                Console.Write($"{srtPath[i].Label} -> ");
            
            Console.Write($"{srtPath.Last().Label}");

        }
    }
}
