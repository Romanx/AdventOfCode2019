﻿using Helpers;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayTwo
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 2)
        {
        }

        public void PartOne(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(int.Parse).ToArray();

            memory[1] = 12;
            memory[2] = 2;

            var result = IntcodeComputer.Compute(memory);

            @out.WriteLine($"Computer result: {result}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(int.Parse).ToImmutableArray();

            var scratch = new int[memory.Length];

            foreach (var noun in Enumerable.Range(0, 99))
            {
                foreach (var verb in Enumerable.Range(0, 99))
                {
                    memory.CopyTo(scratch);

                    scratch[1] = noun;
                    scratch[2] = verb;

                    var result = IntcodeComputer.Compute(scratch);

                    if (result == 19690720)
                    {
                        @out.WriteLine($"Noun: {noun}, Verb: {verb}");
                        @out.WriteLine($"Result: 100 * {noun} + {verb} = {100 * noun + verb}");
                    }
                }
            }
        }
    }
}