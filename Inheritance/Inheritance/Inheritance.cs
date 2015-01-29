// Written by Joe Zachary for CS 3500, September 2010
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inherit
{
    public class Tester
    {
        public static void Main(string[] AssemblyLoadEventArgs)
        {
            Dog d = new Dog("Spot", "mutt");
            Console.WriteLine(d.Shout());

            Animal a1 = new Dog("Spot", "mutt");
            Console.WriteLine(a1.Shout());

            Spider s = new Spider("Charlotte", false);
            Console.WriteLine(s.Shout());

            Animal a = new Spider("Charlotte", false);
            Console.WriteLine(a.Shout());

            Console.WriteLine(SpeakSimultaneously(new Speaker[] { d, s, a1, a }));

            Console.ReadLine();
        }


        public static String SpeakSimultaneously(Speaker[] speakers)
        {
            String result = "";
            foreach (Speaker s in speakers)
            {
                result += s.Speak();
            }
            return result;
        }

    }

    /// <summary>
    /// An interface that requires a Speak() method.
    /// </summary>
    public interface Speaker
    {
        String Speak();
    }


    /// <summary>
    /// An Animal object represents various aspects of a real
    /// animal.  This is an abstract class, so an Animal may
    /// not be directly constructed.  Instead, you must
    /// instantiate one of its derived classes.
    /// </summary>
    public abstract class Animal : Speaker
    {
        /// <summary>
        /// Create an Animal with the specified name.
        /// </summary>
        public Animal(String n)
        {
            Name = n;
        }

        /// <summary>
        /// Obtain the name of the Animal.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// All Animals say something.  This is abstract, which
        /// will force derived classes to override.
        /// </summary>
        /// <returns></returns>
        public abstract String Speak();

        /// <summary>
        /// An upper-case version of what the animal Speaks
        /// </summary>
        /// <returns></returns>
        public String Shout()
        {
            return Speak().ToUpper();
        }

        /// <summary>
        /// By default, an Animal has 8 legs.
        /// </summary>
        public virtual int LegCount
        {
            get { return 8; }
        }

        /// <summary>
        /// Returns what an animal says when it speaks, repeated n times,
        /// where n > 0.  Behavior is unspecified when n <= 0.
        /// </summary>
        public virtual String SpeakRepeatedly(int n)
        {
            String result = "";
            while (n > 0)
            {
                result += Speak() + " ";
                n--;
            }
            return result;
        }

    }


    /// <summary>
    /// A Dog is a kind of Animal
    /// </summary>
    public class Dog : Animal
    {
        /// <summary>
        /// A Dog has a name and a breed
        /// </summary>
        public Dog(String n, String b)
            : base(n)
        {
            Breed = b;
        }

        /// <summary>
        /// Obtain the breed of the Dog
        /// </summary>
        public String Breed { get; private set; }

        /// <summary>
        /// What a dog says.
        /// </summary>
        /// <returns></returns>
        public override String Speak()
        {
            return "woof";
        }

        /// <summary>
        /// This property is now sealed and cannot be overridden.
        /// </summary>
        public override sealed int LegCount
        {
            get
            {
                return 4;
            }
        }

        /// <summary>
        /// Returns what an animal says when it speaks, repeated n times,
        /// where n >= 1. 
        /// </summary>
        public override string SpeakRepeatedly(int n)
        {
            if (n < 1)
            {
                return "-------";
            }
            else
            {
                return base.SpeakRepeatedly(n);
            }
        }
    }



    /// <summary>
    /// A Spider is a kind of Animal
    /// </summary>
    public class Spider : Animal
    {

        /// <summary>
        /// A Spider has a name and may be poisonous
        /// </summary>
        public Spider(String n, bool p)
            : base(n)
        {
            IsPoisonous = p;
        }

        /// <summary>
        /// Find out whether the spider is poisonous
        /// </summary>
        public bool IsPoisonous { get; private set; }

        /// <summary>
        /// What a spider says.
        /// </summary>
        /// <returns></returns>
        public override String Speak()
        {
            return "spin";
        }

        /// <summary>
        /// Attempt to override the non-virtual, non-abstract Shout.
        /// </summary>
        /// <returns></returns>
        public new string Shout()
        {
            return base.Shout() + "!!!!!!";
        }

    }


    /// <summary>
    /// A Cat isn't an animal, but it is a speaker.
    /// </summary>
    public class Cat : Speaker
    {
        public String Speak()
        {
            return "meow";
        }
    }

}
