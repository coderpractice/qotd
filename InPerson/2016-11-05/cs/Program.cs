using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    //Given a binary tree, write a function to get the maximum width of the given tree. Width of a tree is maximum of widths of all levels.
    public partial class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<int,int> tree = new Dictionary<int,int>();
            tree.Add(0, 1);
            tree.Add(1, 2);
            tree.Add(2, 3);
            tree.Add(3, 4);
            tree.Add(4, 5);
            tree.Add(6, 9);
            tree.Add(13, 10);
            tree.Add(14, 11);

           int result = MaxWidth(tree);
           Console.WriteLine(result);

        }
        private static int MaxWidth(Dictionary<int,int> tree) 
        {
            if(tree.Count == 0) 
            {
                return 0;
            }
            List<int> nodes = new List<int>();
            nodes.Add(0);
            return MaxWidth(tree, nodes);
        }

        private static int MaxWidth(Dictionary<int,int> tree, List<int> nodes)
        {
            if(nodes.Count == 0)
            {
                return 0;
            }
            List<int> nextLevel = new List<int>();
            foreach(var index in nodes)
            {
                nextLevel.AddRange(GetChildIndexs(tree, index));
            }
            return Math.Max(nodes.Count, MaxWidth(tree, nextLevel));
        }

        private static List<int> GetChildIndexs(Dictionary<int,int> tree, int index)
        {
            int leftChild = index *2 +1;
            int rightChild = index * 2 +2;
            List<int> result = new List<int>();
            if(tree.ContainsKey(leftChild))
            {
                result.Add(leftChild);
            }

            if(tree.ContainsKey(rightChild))
            {
                result.Add(rightChild);
            }

            return result;
        }
    }    
}
