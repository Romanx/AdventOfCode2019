using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DayFour
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base (day: 4)
        {
        }

        public override string Name => "Secure Container";

        public void PartOne(string input, TextWriter @out)
        {
            var (start, end) = ParseInput(input);

            var possiblePasswords = PossiblePasswords(start, end)
                .Where(p => p.Length == 6)
                .Where(HasAdjacentDigits)
                .Where(EachDigitIncrements)
                .Count();

            @out.WriteLine($"Number of Possible passwords: {possiblePasswords}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var (start, end) = ParseInput(input);

            var possiblePasswords = PossiblePasswords(start, end)
                .Where(p => p.Length == 6)
                .Where(HasAdjacentDigitsNotInGroup)
                .Where(EachDigitIncrements)
                .Count();

            @out.WriteLine($"Number of Possible passwords: {possiblePasswords}");
        }

        private IEnumerable<string> PossiblePasswords(int start, int end)
        {
            var i = start;
            while (i != end)
            {
                yield return i.ToString();
                i++;
            }
        }

        private bool HasAdjacentDigits(string password)
        {
            for (var i = 0; i < password.Length - 1; i++)
            {
                var a = password[i];
                var b = password[i + 1];
                if (a == b)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasAdjacentDigitsNotInGroup(string password)
        {
            var dict = password.Distinct()
                .ToDictionary(k => k, v => 1);

            for (var i = 0; i < password.Length - 1; i++)
            {
                var a = password[i];
                var b = password[i + 1];
                if (a == b)
                {
                    dict[a]++;
                }
            }

            return dict.Values.Any(i => i == 2);
        }

        private bool EachDigitIncrements(string password)
        {
            for (var i = 0; i < password.Length - 1; i++)
            {
                var a = password[i] - '0';
                var b = password[i + 1] - '0';
                if (b < a)
                {
                    return false;
                }
            }

            return true;
        }

        private (int Start, int End) ParseInput(ReadOnlySpan<char> input)
        {
            var hyphenIndex = input.IndexOf('-');

            var s = input[0..hyphenIndex];
            var e = input[(hyphenIndex + 1)..^0];

            return (int.Parse(s), int.Parse(e));
        }
    }
}
