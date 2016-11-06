using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    //Given a Binary Tree find the length of the longest path which comprises of nodes with consecutive values in increasing order. Every node is considered as a path of length 1.
    public class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<int,int> tree = new Dictionary<int,int>();
            tree.Add(0, 10);
            tree.Add(1, 11);
            tree.Add(2, 9);
            tree.Add(3, 13);
            tree.Add(4, 12);
            tree.Add(5, 13);
            tree.Add(6, 8);


           int[] result = LongestConsecutiveSequence(tree);
           Print(result);

        }

        private static int[] LongestConsecutiveSequence(Dictionary<int,int> tree)
        {
            var results = LongestConsecutiveSequence(tree, 0);
            var result = Max(results);
            
            return Enumerable.Range(result.startValue, result.length).ToArray(); 
        }

        private static Result Max(List<Result> results)
        {
            if(!results.Any())
            {
                return null;
            }
            int max = results[0].length;
            Result candidate = results[0];
            for(int i =1;i<results.Count;i++) {
                if(max < results[i].length) {
                    max = results[i].length;
                    candidate = results[i];
                }
            }
            return candidate;
        }

        private static List<Result> LongestConsecutiveSequence(Dictionary<int,int> tree, int index)
        {
            List<Result> results = new List<Result>();

            if(!tree.ContainsKey(index))
            {
                return results;
            }

            int leftIndex = (index *2)+1;
            int rightIndex = (index *2)+2;
            var leftResult = LongestConsecutiveSequence(tree, leftIndex);
            var rightResult = LongestConsecutiveSequence(tree, rightIndex);
            
            var maxLeft = Max(leftResult);
            var maxRight = Max(rightResult);
            var includedCurrent = false;
            if(maxLeft != null)
            {
                if(maxLeft.startIndex == leftIndex && maxLeft.startValue == tree[index]+1)
                {
                    maxLeft.startIndex = index;
                    maxLeft.startValue = tree[index];
                    maxLeft.length = maxLeft.length+1;
                    results.Add(maxLeft);
                    includedCurrent = true;
                }
                else
                {
                    results.Add(maxLeft);
                }
            }

            if(maxRight != null)
            {
                if(maxRight.startIndex == rightIndex && maxRight.startValue == tree[index]+1)
                {
                    maxRight.startIndex = index;
                    maxRight.startValue = tree[index];
                    maxRight.length = maxRight.length+1;
                    results.Add(maxRight);
                    includedCurrent = true;
                }
                else
                {
                    results.Add(maxRight);
                }
            }

            //if the current node is not included then if either of left or right node contains any sequence than take it
            if(includedCurrent == false)
            {
                 Result current = new Result() {
                     startIndex = index,
                     startValue = tree[index],
                     length = 1
                 };

                 List<Result> temp = new List<Result>();
                 temp.Add(current);
                 var next = leftResult.FirstOrDefault(lr => lr.startIndex == index+1 && lr.startValue == tree[index] +1);
                 if(next != null)
                 {
                     next.startIndex = index;
                     next.startValue = tree[index];
                     next.length = next.length +1;
                     temp.Add(next);
                 }
                 next = rightResult.FirstOrDefault(lr => lr.startIndex == index+1 && lr.startValue == tree[index] +1);
                 if(next != null)
                 {
                     next.startIndex = index;
                     next.startValue = tree[index];
                     next.length = next.length +1;
                     temp.Add(next);
                 }
                 var max = Max(temp);
                 results.Add(max);
            }

            return results;
        }

        
        private static void Print(int[] arr)
        {
            string x = ",";
           Console.WriteLine($"[{string.Join(x, arr)}]");
        }
    }
    class Result
    {
       public int startIndex;
       public int length;
       public int startValue;
    }
    //Given a Binary Tree find the length of the longest path which comprises of nodes with consecutive values in increasing order. Every node is considered as a path of length 1.
    
}
