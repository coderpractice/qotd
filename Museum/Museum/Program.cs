using System;
using System.Collections.Generic;
using System.Linq;

namespace Museum
{
    class Node <T>: IComparable<Node<T>>
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
            return operand1.CompareTo(operand2) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(Node<T> operand1, Node<T> operand2)
        {
            return operand1.CompareTo(operand2) == -1;
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
    
    class Point
    { 
        public int Row { get; }
        public int Col { get; }

        public Point(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public override bool Equals(object obj)
        {
            Point n = obj as Point;
            if(n != null)
            {
                return n.Row == this.Row && n.Col == this.Col;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 31 + this.Row;
            hash = hash * 31 + this.Col;
            return hash;
        }
    }

    class Matrix
    {
        private  int[,] Data { get; set; }
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public HashSet<Point> Locked { get; private set; }
        public Matrix(int rows, int cols, ICollection<Point> locked)
        {
            this.Rows = rows;
            this.Cols = cols;
            Data = new int[rows, cols];
            this.Locked = new HashSet<Point>(locked);
        }

        public int this[Point p]
        {
            get
            {
                return Data[p.Row, p.Col];
            }
            set
            {
                Data[p.Row, p.Col] = value;
            }

        }
        public IReadOnlyCollection<Point> GetNeighbours(Point point)
        {
            Point[] neighbours = new Point[]
            {
                Left(point),
                Right(point),
                Up(point),
                Down(point)
            };
            return neighbours.Where(n => n != null && !Locked.Contains(n)).ToList();
        }

        private Point Left(Point point)
        {
            if(point.Col > 0)
            return new Point(point.Row, point.Col - 1);

            return null;
        }

        private Point Right(Point point)
        {
            if (point.Col < Cols-1)
                return new Point(point.Row, point.Col + 1);

            return null;
        }

        private Point Up(Point point)
        {
            if (point.Row  > 0)
                return new Point(point.Row-1, point.Col);

            return null;
        }

        private Point Down(Point point)
        {
            if (point.Row < Rows - 1)
                return new Point(point.Row+1, point.Col);

            return null;
        }

        public void Print()
        {
            for (int i = 0; i < Rows; i++)
            {
                List<string> rowData = new List<string>();
                for (int j = 0; j < Cols; j++)
                {
                    Point p = new Point(i, j);
                    if (this.Locked.Contains(p))
                    {
                        rowData.Add("L ");
                    }
                    else if (this[p] == 0)
                    {
                        rowData.Add("G ");
                    }
                    else if (this[p] == int.MaxValue)
                    {
                        rowData.Add("E ");
                    }
                    else
                    {
                        rowData.Add(string.Format("{0:00}",  Data[i, j]));
                    }
                }
                Console.WriteLine(string.Join(",", rowData));
            }
        }

        internal void Initialize(int value)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i, j] = value;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
             * 2 3 L 1
             * 1 L 1 G
             * G 1 2 1  
             * */
            Point[] locked = new Point[] {
                new Point(0, 2),
                new Point(1,1)
            };
            Point[] guards = new Point[] {
                new Point(2, 0),
                new Point(1,3)
            };
            Matrix input = new Matrix(3, 4, locked);
            Matrix output = GetRank(input, guards);
            output.Print();
            Console.WriteLine("====================");
            /*
             * 2 3 L L
             * 1 L L G
             * G 1 2 1  
             * */
            locked = new Point[] {
                new Point(0, 2),
                new Point(0, 3),

                new Point(1,1),
                new Point(1, 2)

            };
            guards = new Point[] {
                new Point(2, 0),
                new Point(1,3)
            };
            input = new Matrix(3, 4, locked);
            output = GetRank(input, guards);
            output.Print();
            Console.WriteLine("====================");

            /*
             * L E L L
             * 1 L L G
             * G 1 2 1  
             * */
            locked = new Point[] {
                new Point(0, 2),
                new Point(0, 3),
                new Point(0, 0),

                new Point(1,1),
                new Point(1, 2)

            };
            guards = new Point[] {
                new Point(2, 0),
                new Point(1,3)
            };
            input = new Matrix(3, 4, locked);
            output = GetRank(input, guards);
            output.Print();

        }

        private static Matrix GetRank(Matrix input, Point[] guards)
        {
            MinHeap<Point> minHeap = new Museum.MinHeap<Museum.Point>();
            foreach (var guard in guards)
            {
                minHeap.Add(guard, 0);
            }

            Matrix output = new Museum.Matrix(input.Rows, input.Cols, input.Locked);
            output.Initialize(int.MaxValue);
            HashSet<Point> visited = new HashSet<Museum.Point>();
            while(!minHeap.IsEmpty())
            {
                var next = minHeap.Remove();
                if(!visited.Contains(next.Data))
                {
                    visited.Add(next.Data);
                    output[next.Data] = next.Priority; 
                    var neighbours = input.GetNeighbours(next.Data);
                    foreach (var neighbour in neighbours)
                    {
                        if(output[neighbour] > next.Priority + 1)
                        {
                            minHeap.Add(neighbour, next.Priority + 1);
                        }
                    }
                }
            }
            return output;
        }
    }
}