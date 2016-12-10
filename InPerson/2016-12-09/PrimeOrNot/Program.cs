using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeOrNot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(IsPrime(1));
            Console.WriteLine(IsPrime(0));
            Console.WriteLine(IsPrime(-1));
            Console.WriteLine(IsPrime(2));
            Console.WriteLine(IsPrime(3));
            Console.WriteLine(IsPrime(4));
            Console.WriteLine(IsPrime(17));
        }

        private static string IsPrime(int v)
        {
            bool result = false;
            if(v > 0)
            {
                var sqrt = Math.Sqrt(v);
                result = true;
                for (int i = 2; i <= sqrt; i++)
                {
                    if (v % i == 0)
                    {
                        result = false;
                        break;
                    }
                }
            }
            return $"{nameof(IsPrime)}({v}) = {result}";
        }
    }
}
