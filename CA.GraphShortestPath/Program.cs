using System;
using System.Linq;

namespace CA.GraphShortestPath
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("::: SHORTEST PATH :::\n\r");

            // Build the graph
            var g = LoadGraph01();

            // Calculate the shortest path between two nodes
            int n1Index = 0;
            int n2Index = 2;
            (var srtPath, var distances, var previous, bool cycle)
                = g.ShortestPath(g.Nodes[n1Index], g.Nodes[n2Index]);

            // Output the result in console
            if (cycle)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Warning: Graph has negative weight cycles, " +
                    "so the illustrated path is infinite...and result is wrong.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }    

            Console.Write($"Shortest Path " +
                $"from ({g.Nodes[n1Index].Label}) " +
                $"to ({g.Nodes[n2Index].Label}):\t");

            for (int i = 0; i < srtPath.Count - 1; i++)
                Console.Write($"{srtPath[i].Label} -> ");
            
            Console.WriteLine($"{srtPath.Last().Label}\n\r");

            Console.WriteLine("Distances: \t\t\t[{0}]\n\r", string.Join(", ", distances));
            Console.WriteLine("Previous (-1 is source): \t[{0}]", string.Join(", ", previous));

        }

        static Graph<string> LoadGraph01()
        {
            // Directed and weighted graph
            // No edges with negative weights
            // No negative cycles
            // Dijsktra will report a correct shortest path
            var g = new Graph<string>();

            var nA = g.AddNode("A");
            var nB = g.AddNode("B");
            var nC = g.AddNode("C");
            var nD = g.AddNode("D");
            var nE = g.AddNode("E");

            g.Connect(nA, nB, 10);
            g.Connect(nA, nD, 5);

            g.Connect(nB, nA, 1);
            g.Connect(nB, nC, 1);
            g.Connect(nB, nD, 2);

            g.Connect(nC, nB, 1);
            g.Connect(nC, nE, 4);

            g.Connect(nD, nB, 3);
            g.Connect(nD, nC, 9);
            g.Connect(nD, nE, 2);

            g.Connect(nE, nA, 15);
            g.Connect(nE, nC, 6);

            return g;
        }

        static Graph<string> LoadGraph02()
        {
            // Directed and weighted graph
            // Has edges with negative weights
            // Has negative cycles
            // Bellmann-Ford won't report a correct shortest path
            var g = new Graph<string>();

            var nA = g.AddNode("A");
            var nB = g.AddNode("B");
            var nC = g.AddNode("C");
            var nD = g.AddNode("D");
            var nE = g.AddNode("E");
            var nF = g.AddNode("F");

            g.Connect(nA, nB, 2);
            g.Connect(nB, nC, 2);
            g.Connect(nC, nD, 2);

            
            g.Connect(nD, nE, 2);
            g.Connect(nD, nF, -3);

            g.Connect(nF, nC, -3);

            return g;
        }

        static Graph<string> LoadGraph03()
        {
            // Directed and weighted graph
            // Has edges with negative weights
            // Has negative cycles
            // Bellmann-Ford won't report a correct shortest path
            var g = new Graph<string>();

            var nA = g.AddNode("A");
            var nB = g.AddNode("B");
            var nC = g.AddNode("C");
            var nD = g.AddNode("D");
            var nE = g.AddNode("E");
            var nF = g.AddNode("F");
            var nG = g.AddNode("G");

            g.Connect(nA, nB, 1); g.Connect(nA, nC, 1);
            g.Connect(nB, nD, 4);
            g.Connect(nC, nB, 1);
            g.Connect(nD, nC, -6); g.Connect(nD, nE, 1); g.Connect(nD, nF, 1);
            g.Connect(nE, nF, 1); g.Connect(nE, nG, 1);
            g.Connect(nF, nG, 1);

            return g;
        }

        static Graph<string> LoadGraph04()
        {
            // Directed and weighted graph
            // Has edges with negative weights
            // No negative cycles
            // Bellmann-Ford will report a correct shortest path
            var g = new Graph<string>();

            var nA = g.AddNode("A");
            var nB = g.AddNode("B");
            var nC = g.AddNode("C");
            var nD = g.AddNode("D");
            var nE = g.AddNode("E");

            g.Connect(nA, nB, -1); g.Connect(nA, nC, 4);
            g.Connect(nB, nC, 3); g.Connect(nB, nD, 2); g.Connect(nB, nE, 2);
            g.Connect(nD, nB, 1); g.Connect(nD, nC, 5);
            g.Connect(nE, nD, -3);

            return g;
        }

    }

    
}
