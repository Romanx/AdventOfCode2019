using Helpers;
using System.Collections.Concurrent;
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
            var memory = input.Split(',').Select(int.Parse)
                .ToImmutableArray();

            var computer = new IntcodeComputer(memory);
            computer.Input.Enqueue(1);

            computer.Run();

            var output = computer.Output.Last();

            @out.WriteLine($"Computer result: {output}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(int.Parse)
                .ToImmutableArray();

            var computer = new IntcodeComputer(memory);
            computer.Input.Enqueue(5);

            computer.Run();

            var output = computer.Output.Last();

            @out.WriteLine($"Computer result: {output}");
        }
    }
}
