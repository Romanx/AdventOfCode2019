using Helpers;
using System;
using System.IO;

namespace DayTwenty
{
    public class Challenge : ChallengeBase, INeedLines
    {
        public Challenge() : base (day: 20)
        {
        }

        public override string Name => "Donut Maze";

        public void PartOne(string[] input, TextWriter @out)
        {
            var map = Parser.Parse(input);
            var start = map.FindPortal("AA");
            var end = map.FindPortal("ZZ");

            var distance = map.FindDistance(start.Position, end.Position);

            @out.WriteLine($"Path length is: {distance}");
        }

        public void PartTwo(string[] input, TextWriter @out)
        {
            var map = Parser.Parse(input);
            var start = map.FindPortal("AA");
            var end = map.FindPortal("ZZ");

            var distance = map.FindDistanceWithRecursion(start.Position, end.Position);

            @out.WriteLine($"Path length in recursive maze is: {distance}");
        }
    }
}
