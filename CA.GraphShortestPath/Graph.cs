using System;
using System.Collections.Generic;
using System.Text;

namespace CA.GraphShortestPath
{
    /// <summary>
    /// Directed and weighted graph
    /// </summary>
    /// <typeparam name="TNode">Actual data in the node (e.g., int, string, other)</typeparam>
    class Graph<TNode>
    {

        #region Fields and Properties
        private List<Node<TNode>> _nodes;
        public List<Node<TNode>> Nodes { get => _nodes; private set => _nodes = value; }
        #endregion

        #region Constructor(s)
        public Graph()
        {
            Nodes = new List<Node<TNode>>();
        }
        #endregion

        #region Building Process
        public Node<TNode> AddNode(TNode label)
        {
            var node = new Node<TNode>(label);
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

        public void Connect(Node<TNode> n1, Node<TNode> n2, int weight)
        {
            n1.Neighbors.Add(n2);
            n1.Weights.Add(weight);
        }
        #endregion

        #region Shortest Path
        public (List<Node<TNode>>, double[], int[], bool) ShortestPath(Node<TNode> src, Node<TNode> dst)
        {
            List<Node<TNode>> srtPath = new List<Node<TNode>>();
            double[] distances;
            int[] previous;
            bool cycle = false;

            if (IsNegativeWeight())
            {
                // Bellman-Ford
                (distances, previous, cycle) = BellmanFord(src.GraphIndex);
            }
            else
            {
                // Dijsktra
                (distances, previous) = Dijsktra(src);
            }

            previous[src.GraphIndex] = -1;
            BuildPath(srtPath, dst.GraphIndex, previous, 0);
            srtPath.Reverse();
            return (srtPath, distances, previous, cycle);
        }

        private void BuildPath(List<Node<TNode>> srtPath, int dstIndex, int[] previous, int count)
        {
            if ((previous[dstIndex] == -1) || (count == 100)) // when cycles we only run this function 100 times
            {
                srtPath.Add(Nodes[dstIndex]);
                return;
            }
            else
            {
                count++;
                srtPath.Add(Nodes[dstIndex]);
                BuildPath(srtPath, previous[dstIndex], previous, count);
            }
        }
        #endregion

        #region Shortest Path Dijsktra (all edge weights positive)
        public (double[], int[]) Dijsktra(Node<TNode> src)
        {
            int N = Nodes.Count;
            int count = 0;
            bool[] visited = new bool[N];
            double[] distances = new double[N];
            int[] previous = new int[N];

            // Set all distances to "Inf" except src node to 0
            for (int i = 0; i < N; i++)
                distances[i] = double.PositiveInfinity;

            distances[src.GraphIndex] = 0;

            Node<TNode> node;
            int neighborLocalIndex;
            double newDist;
            int index = src.GraphIndex;
            int weight;

            while (count < N) // At least 1 node to visit
            {
                // Visit a node and update all neighborhood
                node = Nodes[index];
                visited[index] = true;
                count++;
                foreach (var neighbor in node.Neighbors)
                {
                    if (!visited[neighbor.GraphIndex]) // Neighbor is not already visited
                    {
                        neighborLocalIndex = node.Neighbors.FindIndex(x => x == neighbor);
                        weight = node.Weights[neighborLocalIndex];
                        newDist = distances[node.GraphIndex] + weight;
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

        private int PickNextNodeIndex(double[] distances, bool[] visited)
        {
            int N = Nodes.Count;
            double currentMin = double.PositiveInfinity;
            int currentMinIndex = 0;
            for (int i = 0; i < N; i++)
            {
                if ((!visited[i]) && (distances[i] < currentMin))
                {
                    currentMin = distances[i];
                    currentMinIndex = i;
                }
            }
            return currentMinIndex;
        }
        #endregion

        #region Shortest Path Bellman-Ford (at least one or more edge weights negative, i.e., potential negative cycles)
        public (double[], int[], bool) BellmanFord(int srcIndex)
        {
            int N = Nodes.Count;
            double[] distances = new double[N];
            int[] previous = new int[N];
            for (int i = 0; i < N; i++)
                distances[i] = Double.PositiveInfinity;
            distances[srcIndex] = 0;

            var edges = GetAllEdges();
            Node<TNode> uNode, vNode;
            int uIndex, vIndex, vLocalIndex, uvWeight;
            double newDist;
            for (int k = 1; k <= N - 1; k++)
            {
                foreach (var edge in edges)
                {
                    uNode = edge.Src;
                    vNode = edge.Dst;
                    uIndex = uNode.GraphIndex;
                    vIndex = vNode.GraphIndex;
                    vLocalIndex = uNode.Neighbors.FindIndex(x => x == vNode);
                    uvWeight = uNode.Weights[vLocalIndex];
                    newDist = distances[uIndex] + uvWeight;
                    if (newDist < distances[vIndex])
                    {
                        distances[vIndex] = newDist;
                        previous[vIndex] = uIndex;
                    }
                }
            }

            bool cycle = false;
            for (int k = 1; k <= N - 1; k++)
            {
                foreach (var edge in edges)
                {
                    uNode = edge.Src;
                    vNode = edge.Dst;
                    uIndex = uNode.GraphIndex;
                    vIndex = vNode.GraphIndex;
                    vLocalIndex = uNode.Neighbors.FindIndex(x => x == vNode);
                    uvWeight = uNode.Weights[vLocalIndex];
                    newDist = distances[uIndex] + uvWeight;
                    if (newDist < distances[vIndex])
                    {
                        // Report negative cycle
                        cycle = true;
                        break;
                    }
                }
            }
            

            return (distances, previous, cycle);

        }
        #endregion

        #region Utils
        public List<Edge<TNode>> GetAllEdges()
        {
            List<Edge<TNode>> edges = new List<Edge<TNode>>();
            int i, w;
            foreach (var n1 in Nodes)
            {
                i = 0;
                foreach (var n2 in n1.Neighbors)
                {
                    w = n1.Weights[i++];
                    edges.Add(new Edge<TNode>(n1, n2, w));
                }
            }

            return edges;
        }
        private bool IsNegativeWeight()
        {
            List<Edge<TNode>> edges = new List<Edge<TNode>>();
            int i, w;
            foreach (var n1 in Nodes)
            {
                i = 0;
                foreach (var n2 in n1.Neighbors)
                {
                    w = n1.Weights[i++];
                    if (w < 0) return true;
                }
            }

            return false;
        }
        public override string ToString()
        {
            return $"Directed and weighted graph with {Nodes.Count} nodes.";
        }
        #endregion

    }
}
