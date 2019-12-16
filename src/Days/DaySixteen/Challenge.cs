using Helpers;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DaySixteen
{
    public class Challenge : ChallengeBase, INeedLines
    {
        public Challenge() : base(day: 16)
        {
        }

        public override string Name => "Flawed Frequency Transmission";

        public void PartOne(string[] input, TextWriter @out)
        {
            foreach (var line in input)
            {
                var signal = line.CharsToDigit().ToImmutableArray();
                var phaseCount = 100;
                var result = RunLine(phaseCount, signal);
                var shortened = result.Take(8).ToArray();
                @out.WriteLine($"Final Signal after {phaseCount} phases: {string.Join("", shortened)}");
            }
        }

        public void PartTwo(string[] input, TextWriter @out)
        {
            var line = input[0];
            var offset = int.Parse(line[..7]);
            var actualInput = line.CharsToDigit().Repeat(10000)
                .ToImmutableArray();

            var arr = actualInput.ToArray();
            var newSequence = new int[actualInput.Length];

            var phase = 0;
            while (phase < 100)
            {
                var sum = 0;
                for (var i = arr.Length - 1; i >= arr.Length / 2; i--)
                {
                    sum += arr[i];
                    newSequence[i] = sum % 10;
                }
                arr = newSequence;

                phase++;
            }

            var shortened = arr.Skip(offset).Take(8);

            @out.WriteLine($"Final Signal after {phase} phases with offset of {offset}: {string.Join("", shortened)}");
        }

        private ImmutableArray<int> RunLine(int phaseCount, ImmutableArray<int> startingSignal)
        {
            var signal = startingSignal;
            for (var i = 0; i < phaseCount; i++)
            {
                signal = RunPhase(signal);
                Console.WriteLine(string.Join("", signal));
            }

            return signal;
        }

        private static ImmutableArray<int> RunPhase(ImmutableArray<int> signal)
        {
            var result = ImmutableArray.CreateBuilder<int>();

            for (var i = 0; i < signal.Length; i++)
            {
                result.Add(ApplyPattern(i, signal));
            }

            return result.ToImmutable();

            static int ApplyPattern(int index, ImmutableArray<int> signal)
            {
                var pattern = GeneratePattern(index + 1, signal.Length);

                var total = 0;
                for (var i = 0; i < signal.Length; i++)
                {
                    var value = signal[i];
                    var patternItem = pattern[i];

                    total += value * patternItem;
                }

                return total.DigitAtPosition(1);
            }
        }

        private static readonly ImmutableArray<int> _basePattern = ImmutableArray.Create(0, 1, 0, -1);

        public static ImmutableArray<int> GeneratePattern(int patternCount, int signalCount)
        {
            var res = Enumerable.Empty<int>();

            foreach (var item in _basePattern)
            {
                res = res.Concat(Enumerable.Repeat(item, patternCount));
            }

            return res.Repeat().Skip(1).Take(signalCount).ToImmutableArray();
        }
    }

    public static class StringHelpers
    {
        public static IEnumerable<int> CharsToDigit(this string @in) => @in.Select(c => c - '0');
    }

    public static class NumberHelpers
    {
        public static int DigitAtPosition(this int @in, uint pos) => @in.ToString().AsSpan()[^((int)pos)] - '0';
    }
}
