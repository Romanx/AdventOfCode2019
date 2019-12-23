using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DayTwentyTwo
{
    public class Challenge : ChallengeBase, INeedLines
    {
        public Challenge() : base(day: 22)
        {
        }

        public override string Name => "";

        public void PartOne(string[] input, TextWriter @out)
        {
            const int cards = 10_007;
            var deck = Enumerable.Range(0, cards).ToArray();

            @out.WriteLine($"Card at position 2019: {Array.IndexOf(deck, 2019)}");
            var resultDeck = ShuffleTechniques.RunShuffleTechnique<int>(deck, input);
            @out.WriteLine($"Position of card 2019: {Array.IndexOf(resultDeck, 2019)}");
        }

        public void PartTwo(string[] input, TextWriter @out)
        {
            var deck = MassiveDeck().ToArray();

            for (long i = 0; i < 101_741_582_076_661; i++)
            {
                @out.WriteLine($"Shuffle {i}");
                deck = ShuffleTechniques.RunShuffleTechnique<long>(deck, input);
            }

            @out.WriteLine($"Position 2020 contains card: {deck[2020]}");
        }

        private IEnumerable<long> MassiveDeck()
        {
            const long cards = 119_315_717_514_047;
            for (long i = 0; i < cards; i++)
            {
                yield return i;
            }
        }
    }
}
