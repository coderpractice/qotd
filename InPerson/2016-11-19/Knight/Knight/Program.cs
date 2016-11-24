using System;
using System.Collections.Generic;
using System.Linq;
using Cell = System.Tuple<int, int>;
using CellLevel =  System.Tuple<System.Tuple<int, int>, int>;


namespace Knight
{
    class Program
    {
        /** given a start position of a Knight on chess board, find the minimum number of moves to reach given destination */
        static void Main(string[] args)
        {
            TestAndPrint(0, 0, 0, 0);
            TestAndPrint(0, 0, 0, 1);
            TestAndPrint(0, 0, 7, 7);
            TestAndPrint(0, 0, 0, -1);
        }

        /// <summary>
        /// Converts the values to Tuples and prints the output nicely
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        static void TestAndPrint(int x, int y, int x1, int y1)
        {
            Console.WriteLine($"MinMove(({x},{y}) => ({x1},{y1})) = {MinMove(new Cell(x, y), new Cell(x1, y1))}");
        }
                
        /// <summary>
        /// Calculates minimum number of moves from start and end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        static int MinMove(Cell start, Tuple <int,int> end)
        {
            if(!IsValid(start) || !IsValid(end))
            {
                return -1;
            }

            HashSet<Cell> visited = new HashSet<Cell>(new TupleComparer());
            Queue<CellLevel> queue = new Queue<CellLevel>();
            
            queue.Enqueue(Get(start, 0));
            while (queue.Count > 0)
            {
                CellLevel current = queue.Dequeue();
                if(TupleComparer.AreEqual(current.Item1,end))
                {
                    return current.Item2;
                }

                if(visited.Contains(current.Item1))
                {
                    continue;
                }
                visited.Add(current.Item1);
                IReadOnlyCollection<Cell> neighbours = GetNeighbours(current.Item1).Where(n => !visited.Contains(n)).ToList();
                queue.Enqueue(neighbours.Select(n => Get(n, current.Item2 + 1)));
            }

            return -1;
        }

        /// <summary>
        /// Gets Cell+Level
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private static CellLevel Get(Cell cell, int level)
        {
            return new CellLevel(cell, level);
        }

        private static IReadOnlyCollection<Cell> GetNeighbours(Cell start)
        {
            List<Cell> neighbours = new List<Tuple<int, int>>();
            //Top left and right
            neighbours.Add(Adjust(start, -2, -1));
            neighbours.Add(Adjust(start, -2, 1));

            //Bottom left and right
            neighbours.Add(Adjust(start, 2, -1));
            neighbours.Add(Adjust(start, 2, 1));

            //Left -> Top, bottom
            neighbours.Add(Adjust(start, -1, -2));
            neighbours.Add(Adjust(start, -1, 2));

            //Right -> Top, bottom
            neighbours.Add(Adjust(start, 1, -2));
            neighbours.Add(Adjust(start, 1, 2));
            return neighbours.Where(n => IsValid(n)).ToList();
        }

        private static Cell Adjust(Cell cell, int row, int col)
        {
            return new Tuple<int, int>(cell.Item1 + row, cell.Item2 + col);
        }

        private static bool IsValid(Cell cell)
        {
            return InBound(cell.Item1) && InBound(cell.Item2);
        }

        private static bool InBound(int dim)
        {
            return dim >= 0 && dim <= 7;
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
    }
}
