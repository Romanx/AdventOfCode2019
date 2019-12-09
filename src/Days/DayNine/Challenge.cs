using Helpers;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayNine
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 9)
        {
        }

        public override string Name => "Sensor Boost";

        public void PartOne(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(long.Parse).ToImmutableArray();
            var computer = new IntcodeComputer(memory);
            computer.Input.Enqueue(1);

            computer.Run();

            @out.WriteLine($"Output: {string.Join(", ", computer.Output)}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(long.Parse).ToImmutableArray();
            var computer = new IntcodeComputer(memory);
            computer.Input.Enqueue(2);

            computer.Run();

            @out.WriteLine($"Output: {string.Join(", ", computer.Output)}");
        }
    }
}
