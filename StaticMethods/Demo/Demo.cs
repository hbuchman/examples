﻿// Written by Joe Zachary for CS 3500

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hailstone;

namespace Demo
{
    static class Demo
    {
        /// <summary>
        /// This is an example of a program that makes use of a DLL created
        /// in a different project in the same solution.  It also makes use
        /// (in Intellisense) of the method documentation.  To make this work,
        /// I right clicked on the project in Solution Explorer and added
        /// a reference to the Hailstone project
        /// 
        /// </summary>
        static void Main(string[] args)
        {
            // This shows how to consume the integers produced by
            // the result of Hail.hailstone(7), which is an IEnumerable<int>.
            foreach (int n in Hail.hailstone(7))
            {
                Console.WriteLine(n);
            }

            // Obtains and prints the first item in the sequence.
            Console.WriteLine(Hail.hailstone(7).First());

            // Obtains and prints the largest item in the sequence.
            Console.WriteLine(Hail.hailstone(7).Max());

            // Prints the reverse of the sequence
            foreach (int n in Hail.hailstone(7).Reverse())
            {
                Console.WriteLine(n);
            }
        }
    }
}
