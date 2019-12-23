using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DayTwentyTwo
{
    public static class ShuffleTechniques
    {
        private static readonly Regex IncrementRegex = new Regex("deal with increment ([0-9]+)");
        private static readonly Regex CutRegex = new Regex("cut (-?[0-9]+)");

        public static T[] RunShuffleTechnique<T>(in ReadOnlySpan<T> deck, string[] commands)
        {
            var result = new T[deck.Length];
            deck.CopyTo(result);

            foreach (var command in commands)
            {
                result = command switch
                {
                    _ when IncrementRegex.TryMatch(command, out var arg) => DealWithIncrementN(result, int.Parse(arg)),
                    _ when CutRegex.TryMatch(command, out var arg) => CutN(result, int.Parse(arg)),
                    "deal into new stack" => DealIntoNewStack(result),
                    _ => throw new InvalidOperationException("Unknown command"),
                };
            }

            return result;
        }

        public static T[] DealIntoNewStack<T>(T[] deck) => deck.Reverse().ToArray();

        public static T[] CutN<T>(T[] deck, int count)
        {
            if (count > 0)
            {
                var cut = deck[..count];
                var rest = deck[count..];

                return rest.Concat(cut).ToArray();
            }
            else
            {
                var abs = Math.Abs(count);

                var cut = deck[^abs..];
                var rest = deck[..^abs];

                return cut.Concat(rest).ToArray();
            }
        }

        public static long CutN(long index, long deckSize, long count) => (index + count + deckSize) % deckSize;

        public static long DealIntoNewStack(long index, long deckSize) => (deckSize - 1) - index;

        public static long DealWithIncrementN(long index, long deckSize, long count) => (index + count) % deckSize;

        public static T[] DealWithIncrementN<T>(T[] deck, long value)
        {
            var copy = new T[deck.Length];
            var pos = 0L;
            foreach (var card in deck)
            {
                copy[pos % deck.Length] = card;
                pos += value;
            }

            return copy;
        }
    }
}
