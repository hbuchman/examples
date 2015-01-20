// Written by Joe Zachary for CS 3500

// Just as you import packages in Java, you use namespaces in C#.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// A namespace organizes classes hierarchically, much as Java packages do.  The key difference
// is that packaged classes are organized into like-named folders.  Namespaces have nothing
// to do with folders.
namespace Hailstone
{
    /// <summary>
    /// Provides hailstone generator.  This project is a class library, and
    /// when it is built it generates a DLL that can be used in other
    /// programs.  To ask Visual Studio to generate an XML file containing
    /// the documentation provided by comments such as this, right click
    /// on the project and go
    ///   Properties > Build > Xml documentation file
    /// </summary>
    public static class Hail
    {
        /// <summary>
        /// Returns an IEnumerable that can generate the hailstone
        /// sequence beginning at n.  The numbers in the sequence
        /// are not generated all at once.  Instead, they are generated
        /// as they are requested by the consumer.
        /// </summary>
        /// <param name="n">Starting point</param>
        public static IEnumerable<int> hailstone (int n)
        {
            while (true)
            {
                yield return n;
                if (n == 1)
                {
                    break;
                }
                else if (n % 2 == 0)
                {
                    n = n / 2;
                }
                else
                {
                    n = 3 * n + 1;
                }
            }
        }
    }
}
