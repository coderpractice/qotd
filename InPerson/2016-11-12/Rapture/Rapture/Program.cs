using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapture
{
    class Node<T> : IComparable<Node<T>>
    {
        public T Data { get; private set; }
        public int Priority { get; private set; }
        public Node(T data, int priority)
        {
            this.Data = data;
            this.Priority = priority;
        }
        public static void Swap(Node<T> d, Node<T> d1)
        {
            Node<T> temp = new Node<T>(d.Data, d.Priority);
            d.Data = d1.Data;
            d.Priority = d1.Priority;

            d1.Data = temp.Data;
            d1.Priority = temp.Priority;
        }

        public int CompareTo(Node<T> other)
        {
            return this.Priority - other.Priority;
        }

        // Define the is greater than operator.
        public static bool operator >(Node<T> operand1, Node<T> operand2)
        {
            return operand1.CompareTo(operand2) > 0;
        }

        // Define the is less than operator.
        public static bool operator <(Node<T> operand1, Node<T> operand2)
        {
            return operand1.CompareTo(operand2) < 0;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(Node<T> operand1, Node<T> operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(Node<T> operand1, Node<T> operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }
        public override string ToString()
        {
            return $"{Data}, {Priority}";
        }
    }


    class MinHeap<T>
    {
        private int GetParent(int index)
        {
            return index / 2;
        }
        List<Node<T>> queue = new List<Node<T>>();
        int tail = 0;
        public void Add(T data, int priority)
        {
            queue.Add(new Node<T>(data, priority));
            while (tail > 0 && queue[GetParent(tail)] > queue[tail])
            {
                Node<T>.Swap(queue[GetParent(tail)], queue[tail]);
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

        public Node<T> Remove()
        {
            Node<T>.Swap(queue[0], queue[tail - 1]);
            Node<T> ret = queue[tail - 1];
            queue.RemoveAt(tail - 1);
            tail--;

            int current = 0;
            //Fix the min heap
            while (current < tail)
            {
                int left = LeftChild(current);
                int right = RightChild(current);
                if (left < tail && queue[left] < queue[current])
                {
                    Node<T>.Swap(queue[left], queue[current]);
                    current = left;
                }
                else if (right < tail && queue[right] < queue[current])
                {
                    Node<T>.Swap(queue[right], queue[current]);
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

    class Routes
    {
        Dictionary<int /*Start */, List<Tuple<int, int>>> stations = new Dictionary<int, List<Tuple<int, int>>>();

        public void Add(int start, int destination, int fare)
        {
            List<Tuple<int, int>> neighbours;
            if (!stations.TryGetValue(start, out neighbours))
            {
                neighbours = new List<Tuple<int, int>>();
            }
            neighbours.Add(new Tuple<int, int>(destination, fare));
            stations[start] = neighbours;
        }

        public List<Tuple<int, int>> GetNeighbours(int start)
        {
            return stations[start];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Routes routes = new Routes();

            routes.Add(1, 2, 60);
            routes.Add(3, 5, 70);
            routes.Add(1, 4, 120);
            routes.Add(4, 5, 150);
            routes.Add(2, 3, 80);

            int fare = MinFare(routes, 1, 5);
            Console.WriteLine(fare);
        }

        private static int MinFare(Routes routes, int start, int end)
        {
            MinHeap<int> queue = new MinHeap<int>();
            queue.Add(start, 0);
            HashSet<int> visited = new HashSet<int>();
            while(!queue.IsEmpty())
            {
                var current = queue.Remove();
                visited.Add(current.Data);
                if(current.Data == end)
                {
                    return current.Priority;
                }

                List<Tuple<int, int>> neighbours = routes.GetNeighbours(current.Data);
                foreach (var neighbour in neighbours)
                {
                    if(!visited.Contains(neighbour.Item1))
                    {
                        int additionalFare = (neighbour.Item2 - current.Priority);
                        if (additionalFare < 0)
                        {
                            additionalFare = 0;
                        }
                        int totalFare = current.Priority + additionalFare;

                        queue.Add(neighbour.Item1, totalFare);
                    }
                }
            }

            return int.MaxValue;
        }

        private static Tuple<int, int> GetNeighbours(Dictionary<int, Tuple<int, int>> stations, int data)
        {
            throw new NotImplementedException();
        }
    }
}
