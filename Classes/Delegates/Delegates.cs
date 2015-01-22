// Written by Joe Zachary for CS 3500, January 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LectureExamples
{
    /// <summary>
    /// Examples of how delegates work
    /// </summary>
    public class Delegates
    {
        public static void Main(string[] args)
        {
            // Make a list for demonstration purposes
            List<String> list = new List<String>();
            list.Add("hello");
            list.Add("long string");
            list.Add("a");
            list.Add("another long string");

            // Write out the whole list, and then filter it in various ways
            Console.WriteLine("Entire list: " + String.Join(", ", list));
            Console.WriteLine("Length > 5:  " + String.Join(", ", FilterList(list, LongerThan5)));
            Console.WriteLine("Length == 5: " + String.Join(", ", FilterList(list, s => s.Length == 5)));

            // Methods can be stored in variables
            Finder f = LongerThan5;
            Console.WriteLine(f("joe"));

            // Anonymous methods (lambda expressions) can also be stored in variables
            f = s => s.Length == 3;
            Console.WriteLine(f("joe"));
        }

        /// <summary>
        /// Reports whether s contains more than 5 characters
        /// </summary>
        public static bool LongerThan5(String s)
        {
            return s.Length > 5;
        }

        /// <summary>
        /// Declaration of a delegate type
        /// </summary>
        public delegate bool Finder(String s);

        /// <summary>
        /// Enumerates the elements of list that satisfy f
        /// </summary>
        public static IEnumerable<String> FilterList(List<String> list, Finder f)
        {
            foreach (String s in list)
            {
                if (f(s))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Another way to declare the type of a delegate
        /// </summary>
        public static IEnumerable<String> FilterList2(List<String> list, Func<String, bool> f)
        {
            foreach (String s in list)
            {
                if (f(s))
                {
                    yield return s;
                }
            }
        }
    }
}
