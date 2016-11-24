using System;
using System.Collections.Generic;
using System.Linq;
using Orange;
using Cell = System.Tuple<int, int>;
using CellLevel = System.Tuple<System.Tuple<int, int>, int>;

namespace Orange
{
    enum State
    {
        Empty = 0,
        Fresh = 1,
        Rotten = 2
    }
    class Program
    {
        //http://www.geeksforgeeks.org/minimum-time-required-so-that-all-oranges-become-rotten/
        static void Main(string[] args)
        {
            State[,] field = new State[,]{
                     { State.Rotten, State.Fresh, State.Empty, State.Rotten, State.Fresh},
                     { State.Fresh, State.Empty, State.Fresh, State.Rotten, State.Fresh},
                     { State.Fresh, State.Empty, State.Empty, State.Rotten, State.Fresh}
            };

            int timeToRot = GetTimeToRot(field);
            Console.WriteLine(timeToRot);

            field = new State[,]{
                     { State.Rotten, State.Fresh, State.Empty, State.Rotten, State.Fresh},
                     { State.Empty, State.Empty, State.Fresh, State.Rotten, State.Fresh},
                     { State.Fresh, State.Empty, State.Empty, State.Rotten, State.Fresh}
            };

            timeToRot = GetTimeToRot(field);
            Console.WriteLine(timeToRot);

        }

        private static int GetTimeToRot(State[,] field)
        {
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);
            HashSet<Cell> rotten = new HashSet<Cell>(new TupleComparer());
            HashSet<Cell> fresh = new HashSet<Cell>(new TupleComparer());
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    switch (field[i, j])
                    {
                        case State.Empty:
                            break;
                        case State.Fresh:
                            fresh.Add(new Cell(i, j));
                            break;
                        case State.Rotten:
                            rotten.Add(new Cell(i, j));
                            break;
                        default:
                            break;
                    }
                }
            }

            if (rotten.Count == 0 && fresh.Count > 0)
            {
                return int.MaxValue;
            }

            if (fresh.Count == 0)
            {
                return 0;
            }

            Queue<CellLevel> queue = new Queue<CellLevel>();
            HashSet<Cell> visited = new HashSet<Tuple<int, int>>();
            queue.Enqueue(rotten, 0);
            int max = 0;

            while(queue.Count > 0 )
            {
                CellLevel current = queue.Dequeue();
                if(visited.Contains(current.Item1))
                {
                    continue;
                }
                max = current.Item2;
                visited.Add(current.Item1);
                if(fresh.Contains(current.Item1))
                {
                    fresh.Remove(current.Item1);
                }
                var neighbours = GetNeighbours(current.Item1, rows, cols).Where(n => field[n.Item1, n.Item2] == State.Fresh);
                queue.Enqueue(neighbours, current.Item2 + 1);
            }

            if(fresh.Count >0)
            {
                return int.MaxValue;
            }
            return max;
        }

        private static bool IsValid(Cell cell, int rows, int cols)
        {
            return InBound(cell.Item1, rows) && InBound(cell.Item2, cols);
        }

        private static bool InBound(int dim, int bound)
        {
            return dim >= 0 && dim < bound;
        }

        private static CellLevel Get(Cell cell, int level)
        {
            return new CellLevel(cell, level);
        }

        private static IReadOnlyCollection<Cell> GetNeighbours(Cell start, int rows, int cols)
        {
            List<Cell> neighbours = new List<Tuple<int, int>>();
            neighbours.Add(Adjust(start, 0, 1));
            neighbours.Add(Adjust(start, 0, -1));

            neighbours.Add(Adjust(start, 1, 0));
            neighbours.Add(Adjust(start, -1, 0));

            return neighbours.Where(n => IsValid(n, rows, cols)).ToList();
        }

        private static Cell Adjust(Cell cell, int row, int col)
        {
            return new Tuple<int, int>(cell.Item1 + row, cell.Item2 + col);
        }
    }

    internal class TupleComparer : IEqualityComparer<Cell>
    {
        public static bool AreEqual(Cell x, Cell y)
        {
            return x.Item1 == y.Item1 && x.Item2 == y.Item2;
        }
        public bool Equals(Cell x, Cell y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(Cell obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.Item1;
            hash = hash * 31 + obj.Item2;
            return hash;
        }
    }

    public static class QueueExtensions
    {
        public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> values)
        {
            foreach (var item in values)
            {
                queue.Enqueue((item));
            }
        }

        public static void Enqueue(this Queue<CellLevel> queue, IEnumerable<Cell> values, int level)
        {
            foreach (var item in values)
            {
                queue.Enqueue(Get(item, level));
            }
        }

        private static CellLevel Get(Cell cell, int level)
        {
            return new CellLevel(cell, level);
        }
    }
}
