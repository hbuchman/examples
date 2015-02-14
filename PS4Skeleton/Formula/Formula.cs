// Skeleton written by Joe Zachary for CS 3500, January 2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// 
    /// Associated with each Formula are two delegates: a normalizer and a validator.  
    /// A normalizer takes a string as a parameter and returns a string as a result; 
    /// its purpose is to convert variables into a canonical form.  A validator takes a
    /// string as a parameter and returns a boolean as a result; its purpose is to impose 
    /// extra restrictions on the validity of a variable.  
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using standard C# syntax for double/int literals), 
        /// variable symbols (an underscore, letter, or digit followed by one or more letters and/or digits),
        /// left and right parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// An example of a valid parameter to this constructor is "2.5e9 + x5 / 17".
        /// Examples of invalid parameters are "x", "-5.3", and "2 5 + 3";
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// 
        /// The normalizer and validator become associated with the Formula, and they play into the
        /// definition of syntactic validity.  Suppose that f, N, and V are passed as the three parameters 
        /// to the constructor.  N becomes the formula's normalizer and V becomes its validator.  If f is 
        /// syntactically correct but contains a variable v such that N(v) is not a legal variable as 
        /// discussed above, the constructor should throw a FormulaFormatException with an explanatory 
        /// message.  Otherwise, if f contains a variable v such that V(N(v)) is false, the constructor 
        /// should throw a FormulaFormatException with an explanatory message.  
        /// </summary>
        public Formula(String formula, Func<string, string> normalizer, Func<string, bool> validator)
        {
        }

        /// <summary>
        /// This behaves like the 3-parameter version of the constructor, except that the normalizer
        /// is the identity function and the validator is a string predicate that always returns true.
        /// </summary>
        public Formula(String formula)
        {
        }


        /// <summary>
        /// Evaluates this Formula, using lookup to determine the values of variables.  Variables
        /// are looked up via their normalized forms.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, throw a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            return 0;
        }

        /// <summary>
        /// Enumerates the normalized forms of all the variables that appear in this Formula.  Each
        /// distinct normalized variable occurs exactly once.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetVariables ()
        {
            return null;
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of one or more
        /// letters followed by one or more digits, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z]+\d+";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }


    /// <summary>
    /// A Lookup function is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an ArgumentException (meaning that the string is unmapped.
    /// Exactly how a Lookup function decides which strings map to doubles and which
    /// don't is up to the implementation of that function.
    /// </summary>
    public delegate double Lookup(string s);

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message)
            : base(message)
        {
        }
    }
}
