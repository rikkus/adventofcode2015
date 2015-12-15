using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc
{    
    internal class Program
    {
        private static readonly Regex ThreeLetters = new Regex("[a-z]{3}", RegexOptions.Compiled);
        private static readonly Regex RepeatedCharacter = new Regex(@"(.)\1", RegexOptions.Compiled);
        private static readonly Regex Iol = new Regex("[iol]", RegexOptions.Compiled);

        public static string Increment(string s)
        {
            var ret = s.ToCharArray();

            for (var pos = s.Length - 1; pos >= 0; pos--)
            {
                if (s[pos] == 'z')
                {
                    ret[pos] = 'a';
                }
                else
                {
                    ret[pos] = (char)(s[pos]+1);
                    break;
                }
            }

            return new string(ret);
        }

        private static void Main()
        {
            var valid = false;
            var password = "cqjxxyzz";

            while (!valid)
            {
                password = Increment(password);

                valid = IsValid(password);
            }

            Console.WriteLine(password);

            Console.ReadKey();
        }

        private static bool IsValid(string password)
        {
            return ContainsMonotonicallyIncreasingSubstring(password)
                   && !Iol.IsMatch(password)
                   && ContainsTwoDifferentNonOverlappingPairs(password);

        }

        private static bool ContainsMonotonicallyIncreasingSubstring(string password)
        {
            var matchObj = ThreeLetters.Match(password);

            while (matchObj.Success)
            {
                if (matchObj.Value[0] == matchObj.Value[1] - 1 && matchObj.Value[1] == matchObj.Value[2] - 1)
                    return true;

                matchObj = ThreeLetters.Match(password, matchObj.Index + 1);

            }
            return false;
        }

        private static bool ContainsTwoDifferentNonOverlappingPairs(string password)
        {
            return new HashSet<string>(RepeatedCharacter.Matches(password).Cast<Match>().Select(m => m.Value)).Count > 1;
        }
    }
}



