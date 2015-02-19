using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threads
{
    /// <summary>
    /// Illustrates various aspects of threads
    /// </summary>
    public class Threads
    {
        private static int n = 0;
        private static readonly object sync = new object();

        public static void Main(string[] args)
        {
            AutoResetEvent are1 = new AutoResetEvent(true);
            AutoResetEvent are2 = new AutoResetEvent(false);
            Thread t1 = new Thread(() => count(0, 2, 100, are1, are2));
            Thread t2 = new Thread(() => count(1000, 20, 2000, are2, are1));
            t1.IsBackground = true;
            t2.IsBackground = true;
            t1.Start();
            t2.Start();
            Console.ReadLine();
        }

        public static void count(int start, int increment, int limit,
                                AutoResetEvent a1,  AutoResetEvent a2)
        {
            for (int i = start; i < limit; i += increment)
            {
                a1.WaitOne();
                Console.WriteLine(i);
                a2.Set();
          
            }
            Console.ReadLine();
        }

        public static void increment()
        {
            for (int i = 0; i < 10000000; i++)
            {
                try
                {
                    Monitor.Enter(sync);
                    {
                        n++;
                    }
                }
                finally
                {
                    Monitor.Exit(sync);
                }
            }
        }

        public static void decrement()
        {
            for (int i = 0; i < 10000000; i++)
            {
                try
                {
                    Monitor.Enter(sync);
                    n--;
                }
                finally
                {
                    Monitor.Exit(sync);
                }
            }
        }
    }
}
