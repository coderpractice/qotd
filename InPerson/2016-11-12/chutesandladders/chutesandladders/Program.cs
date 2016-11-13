using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chutesandladders
{
    class Node : IComparable<Node>
    {
        public int Data { get; private set; }
        public int Priority { get; private set; }
        public Node(int data, int priority)
        {
            this.Data = data;
            this.Priority = priority;
        }
        public static void Swap(Node d, Node d1)
        {
            Node temp = new Node(d.Data, d.Priority);
            d.Data = d1.Data;
            d.Priority = d1.Priority;

            d1.Data = temp.Data;
            d1.Priority = temp.Priority;
        }

        public int CompareTo(Node other)
        {
            return this.Priority - other.Priority;
        }

        // Define the is greater than operator.
        public static bool operator >(Node operand1, Node operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(Node operand1, Node operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(Node operand1, Node operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(Node operand1, Node operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }
        public override string ToString()
        {
            return $"{Data}, {Priority}";
        }
    }


    class MinHeap
    {
        private int GetParent(int index)
        {
            return index / 2;
        }
        List<Node> queue = new List<Node>();
        int tail = 0;
        public void Add(int data, int priority)
        {   
            queue.Add(new Node(data, priority));
            while(tail >0 && queue[GetParent(tail)] > queue[tail])
            {
                Node.Swap(queue[GetParent(tail)], queue[tail]);
                tail = GetParent(tail);
            }
            tail++;
        }

        private int LeftChild(int index)
        {
            return (index * 2) + 1;
        }

        private int RightChild(int index)
        {
            return (index * 2) + 2;
        }

        public Node Remove()
        {
            Node.Swap(queue[0], queue[tail-1]);
            Node ret = queue[tail-1];
            queue.RemoveAt(tail-1);
            tail--;

            int current = 0;
            //Fix the min heap
            while (current < tail)
            {
                int left = LeftChild(current);
                int right = RightChild(current);
                if(left < tail && queue[left] < queue[current])
                {
                    Node.Swap(queue[left], queue[current]);
                    current = left;
                }
                else if (right < tail && queue[right] < queue[current])
                {
                    Node.Swap(queue[right], queue[current]);
                    current = right;
                }
                else
                {
                    break;
                }
            }
            return ret;
        }

        public bool IsEmpty()
        {
            return tail == 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int> ladders = new Dictionary<int, int>();
            ladders[1] = 60;
            ladders[50] = 100;
            Dictionary<int, int> chutes = new Dictionary<int, int>();
            chutes[61] = 49;
            int r = MinimumMoves(ladders, chutes);
            Console.WriteLine($"Expected: 3, Actual {r}");


            ladders = new Dictionary<int, int>();
            ladders[1] = 60;
            ladders[2] = 99;
            ladders[50] = 100;
            chutes = new Dictionary<int, int>();
            chutes[61] = 49;
            r = MinimumMoves(ladders, chutes);
            Console.WriteLine($"Expected: 2, Actual {r}");

            ladders = new Dictionary<int, int>();
            chutes = new Dictionary<int, int>();
            r = MinimumMoves(ladders, chutes);
            Console.WriteLine($"Expected: 17, Actual {r}");

            ladders = new Dictionary<int, int>();
            chutes = new Dictionary<int, int>();
            chutes[99] = 1;
            chutes[98] = 1;
            chutes[97] = 1;
            chutes[96] = 1;
            chutes[95] = 1;
            chutes[94] = 1;
            r = MinimumMoves(ladders, chutes);
            Console.WriteLine($"Expected: -1, Actual {r}");
        }

        static int[] GetNeighbours(int index, Dictionary<int /*Tail */, int /* Head */> ladders,
            Dictionary<int /*Head */, int /*Tail*/> chutes)
        {
            List<int> neighbours = new List<int>();
            for (int i = 1; index +i <= 100 && i < 7; i++)
            {
                int next = index + i;
                int temp;
                if(ladders.TryGetValue(next, out temp))
                {
                    next = temp;
                }
                else if(chutes.TryGetValue(next, out temp))
                {
                    next = temp;
                }
                neighbours.Add(next);
            }
            return neighbours.ToArray();
        }


        static int MinimumMoves(Dictionary<int /*Tail */, int /* Head */> ladders, 
            Dictionary<int /*Head */,int /*Tail*/> chutes)
        {
            Dictionary<int /* cell */, int /*distance*/> distances = new Dictionary<int, int>();
            HashSet<int> visited = new HashSet<int>();
            MinHeap minHeap = new MinHeap();
            minHeap.Add(0, 0);

            //Runs through basic Dijkastra's algorithm for shortest distance
            while (!minHeap.IsEmpty())
            {
                Node current = minHeap.Remove();
                
                //Get all unvisited neighbours
                var neighbours = GetNeighbours(current.Data, ladders, chutes).Where(n => !visited.Contains(n)).ToArray();

                foreach (var neighbour in neighbours)
                {
                    //If we reached 100 then return
                    if (neighbour == 100)
                    {
                        return current.Priority + 1;
                    }
                    int currDist;
                    //Update the distance of the neighbour if > current.Priority +1 or if not set yet
                    if (!distances.TryGetValue(neighbour, out currDist) || currDist > current.Priority + 1)
                    {
                        distances[neighbour] = current.Priority + 1;
                        minHeap.Add(neighbour, current.Priority + 1);
                    }
                }
            }
            return -1;
        } 
    }
}
