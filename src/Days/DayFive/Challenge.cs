using Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayFive
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 5)
        {
        }

        public override string Name => "Sunny with a Chance of Asteroids";

        public void PartOne(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(int.Parse);

            var inputVars = new Stack<int>();
            inputVars.Push(1);
            var runTime = new Runtime(inputVars);

            var result = IntcodeComputer.Compute(memory.ToImmutableArray(), runTime);

            var output = runTime.Output.Pop();

            @out.WriteLine($"Computer result: {output}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(int.Parse);

            var inputVars = new Stack<int>();
            inputVars.Push(5);
            var runTime = new Runtime(inputVars);
            var result = IntcodeComputer.Compute(memory.ToImmutableArray(), runTime);

            var output = runTime.Output.Pop();

            @out.WriteLine($"Computer result: {output}");
        }
    }
}
