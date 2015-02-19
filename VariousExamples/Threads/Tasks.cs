using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;

namespace TaskDemo
{
    /// <summary>
    /// Tasks are an abstraction that C# provides to make low-level threads
    /// easier to use.  Here we explore various aspects of tasks.
    /// </summary>
    public class Tasks
    {
        private static int counter = 0;
        private static readonly object sync = new object();

        public static void Main(string[] args)
        {
            //DuelingCounts();

            //DuelingSteps();

            ComputeTwoThings();
        }

        /// <summary>
        /// Uses two threads to print integers 
        /// </summary>
        private static void DuelingCounts()
        {
            Task t1 = Task.Run(() => Count(100, 1, 200));
            Task t2 = Task.Run(() => Count(200, 1, 300));
            t1.Wait();
            t2.Wait();
        }

        /// <summary>
        /// Prints the integers in the specified range.
        /// </summary>
        private static void Count(int start, int increment, int limit)
        {
            for (int i = start; i < limit; i += increment)
            {
                Console.WriteLine(i);
            }
        }

        /// <summary>
        /// Simultaneously increments/decrements a shared variable
        /// </summary>
        private static void DuelingSteps()
        {
            Task t1 = Task.Run(() => Increment(1000000));
            Task t2 = Task.Run(() => Decrement(1000000));
            t1.Wait();
            t2.Wait();
            Console.WriteLine(counter);
        }

        /// <summary>
        /// Increments counter (a shared variable) n times
        /// </summary>
        private static void Increment(int n)
        {
            for (int i = 0; i < n; i++)
            {
                // Only one thread at a time in
                // a critical section
                lock (sync)
                {
                    counter++;
                }
            }
        }

        /// <summary>
        /// Decrements counter (a shared variable) n times
        /// </summary>
        private static void Decrement(int n)
        {
            for (int i = 0; i < n; i++)
            {
                lock (sync)
                {
                    counter--;
                }
            }
        }

        /// <summary>
        /// Calculates the number of digits in two factorials and displays them
        /// </summary>
        private static void ComputeTwoThings()
        {
            Task<BigInteger> task1 = Task<BigInteger>.Run(() => Factorial(30000));
            Task<BigInteger> task2 = Task<BigInteger>.Run(() => Factorial(20000));
            Console.WriteLine(task1.Result.ToString().Length);
            Console.WriteLine(task2.Result.ToString().Length);
        }

        /// <summary>
        /// Returns n!
        /// </summary>
        public static BigInteger Factorial(int n)
        {
            BigInteger result = 1;
            while (n > 0)
            {
                result *= n;
                n--;
            }
            return result;
        }
    }
}
