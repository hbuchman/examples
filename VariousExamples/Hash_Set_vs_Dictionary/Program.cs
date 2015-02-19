using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
 *  Author: H. James de St. Germain
 *  Date:   Fall 2013
 * 
 *
 *  This sample code is used to show features of the Hash Set
 *  and a contained class "Pair".
 * 
 *   Pair is immutable and we want the following behavior:
 *  
 *      new Pair(a,b) == new Pair(a,b)
 * 
 *   To do this you must override:
 * 
 *        Equals
 *    and
 *        GetHashCode!!!!  (otherwise containers like hash set won't know
 *                          to use .Equals)
 */


namespace Hash_Set_vs_Dictionary
{
    class Pair
    {
        /**
         *  false - use base object.Equals code
         *  true  - use pair definition (immutable value equality) of .Equals
         */
        const bool USE_OBJECT_EQUALS = false;


        public String a;
        public String b;

        public Pair(string p1, string p2)
        {
            this.a = p1;
            this.b = p2;
        }

        public override bool Equals(object other)
        {
            if (USE_OBJECT_EQUALS)
            {
                return base.Equals(other);
            }

            Pair p = other as Pair;
            Console.WriteLine("in equals");

            if (! ReferenceEquals(p, null) )
            {
                return p.a == a && p.b == b;
            }

            return false;
        }


        public static bool operator==(Pair p1, Pair p2)
        {
            Console.WriteLine("in ==");
            if (ReferenceEquals(p1, null))
            {
                return ReferenceEquals(p2, null);
            }

            if (ReferenceEquals(p2, null))
            {
                return false;
            }

            return p1.Equals(p2);

        }

        public static bool operator!=(Pair p1, Pair p2)
        {
            return ! (p1 == p2);
        }


        public override int GetHashCode()
        {
            return a.GetHashCode() + b.GetHashCode();
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            HashSet<Pair> hash_set = new HashSet<Pair>();

            Pair p1 = new Pair("jim", "dav");
            Pair p2 = new Pair("jim", "dav");

            Console.WriteLine( "is .Equals overwritten? " + p1.Equals(p2) );

            Console.WriteLine("Adding p1(jim,dav) into set");
            hash_set.Add(p1);

            Console.WriteLine("Is p1 in set?  " + hash_set.Contains(p1));
            Console.WriteLine("Is p2 in set?  " + hash_set.Contains(p2));

            Console.WriteLine("Removing new pair(jim,dav) from set");
            hash_set.Remove( new Pair("jim","dav") );

            Console.WriteLine("Is p1 in set?  " + hash_set.Contains(p1));


            Console.Read();

        }
    }
}
