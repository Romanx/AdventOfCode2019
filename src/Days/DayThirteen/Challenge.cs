using Helpers;
using Helpers.Computer;
using System;
using System.IO;
using System.Linq;

namespace DayThirteen
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 13)
        {
        }

        public override string Name => "Care Package";

        public void PartOne(string input, TextWriter @out)
        {
            var memory = IntcodeParser.Parse(input);
            var cabnet = new Cabinet(memory);

            cabnet.Run();
            var wallCount = cabnet.Tiles
                .Values
                .Count(tt => tt == TileType.Block);

            @out.WriteLine($"Number of walls: {wallCount}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var memory = IntcodeParser.Parse(input);
            memory = memory.SetItem(0, 2);
            var cabnet = new Cabinet(memory);
            var score = cabnet.Run();

            @out.WriteLine($"Final Score is: {score}");
        }
    }
}
