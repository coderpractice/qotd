using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneGraph
{
    class Graph
    {
        Dictionary<int, HashSet<int>> m_Nodes = new Dictionary<int, HashSet<int>>();

        public IEnumerable<int> GetNeigbhours(int v)
        {
            HashSet<int> neighbours;
            if (!m_Nodes.TryGetValue(v, out neighbours))
            {
                return new List<int>();
            }

            return neighbours;
        }

        public IEnumerable<int> Vertices()
        {
            return m_Nodes.Keys;
        }

        public void Add(int v1, int v2)
        {
            HashSet<int> neighbours;
            if(!m_Nodes.TryGetValue(v1, out neighbours))
            {
                neighbours = new HashSet<int>();
            }
            neighbours.Add(v2);
            m_Nodes[v1] = neighbours;

            if(!m_Nodes.TryGetValue(v2, out neighbours))
            {
                neighbours = new HashSet<int>();
            }
            neighbours.Add(v1);
            m_Nodes[v2] = neighbours;
        }

        public void Print()
        {
            foreach (var item in m_Nodes.Keys.OrderBy(k => k))
            {
                Console.WriteLine($"{item} => {string.Join(",", m_Nodes[item].OrderBy(v => v))}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Graph g = new Graph();
            g.Add(1, 1);
            g.Add(1, 2);
            g.Add(2, 3);
            g.Add(3, 1);
            g.Add(1, 4);
            g.Add(5, 5); //Disconnected node

            Graph g1 = Clone(g);
            g.Print();
            Console.WriteLine("=======================================================");
            g1.Print();
        }

        private static Graph Clone(Graph g)
        {
            if(g == null)
            {
                return null;
            }
            HashSet<int> visited = new HashSet<int>();
            Queue<int> queue = new Queue<int>();
            Graph g1 = new CloneGraph.Graph();

            foreach (var start in g.Vertices())
            {
                queue.Enqueue(start);
                while (queue.Count > 0)
                {
                    var n = queue.Dequeue();
                    if (visited.Contains(n))
                    {
                        continue;
                    }
                    visited.Add(n);
                    var neighbours = g.GetNeigbhours(n);
                    foreach (var neighbour in neighbours)
                    {
                        g1.Add(n, neighbour);
                        queue.Enqueue(neighbour);
                    }
                }
            }
            return g1;
        }
    }
}
