using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelegateExamples
{
    /// <summary>
    /// Illustrates how delegates (method parameters) work in C#.
    /// </summary>
    static class Delegates
    {
        static void Main(string[] args)
        {
            // We create a list of strings to experiment with
            List<String> list = new List<String>();
            list.Add("17");
            list.Add("22");
            list.Add("47");
            list.Add("3");
            list.Add("15");
            list.Add("28");
            list.Add("19");
            list.Add("231");

            // Alphabetical order
            list.Sort();
            Console.WriteLine("Alphabetical order");
            print(list);

            // Numerical order
            list.Sort(compare);
            Console.WriteLine("Numerical order");
            print(list);

            // Numerical order using lambda expression
            list.Sort((x, y) => Convert.ToInt32(x).CompareTo(Convert.ToInt32(y)));
            Console.WriteLine("Numerical order using lambda");
            print(list);

            // Order by length of strings
            list.Sort((x, y) => x.Length.CompareTo(y.Length));
            Console.WriteLine("String length order");
            print(list);

            // Create a list of numbers
            List<int> numbers = new List<int>();
            numbers.Add(3);
            numbers.Add(4);
            numbers.Add(1);
            numbers.Add(2);

            // Add them all up
            Console.WriteLine(fold(numbers, Math.Max, int.MinValue));
        }

        /// <summary>
        /// Compares two strings according to the integers they represent
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static int compare(String x, String y)
        {
            return Convert.ToInt32(x).CompareTo(Convert.ToInt32(y));
        }

        /// <summary>
        /// Combines the elements of a list by applying a binary function f to the elements
        /// from left to right, using the provided identity element.  For example, if he
        /// list is [1,2,3,4], the method computes f(f(f(f(identity,1),2),3),4).
        /// </summary
        static int fold (List<int> list, Func<int,int,int> f, int identity)
        {
            int result = identity;
            foreach (int n in list)
            {
                result = f(result, n);
            }
            return result;
        }

        /// <summary>
        /// Prints out a list for debbugging purposes
        /// </summary>
        static void print (List<String> list)
        {
            foreach (String s in list)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine("\n");
        }
    }
}
