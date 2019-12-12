using Helpers;
using Helpers.Points;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DayTwelve
{
    public class Challenge : ChallengeBase, INeedLines
    {
        public Challenge() : base(day: 12)
        {
        }

        public override string Name => "The N-Body Problem";

        public void PartOne(string[] input, TextWriter @out)
        {
            var points = ParsePoints(input);

            var finalState = Simulator.Simulate(1000, points);

            var total = finalState
                .Sum(x => CalculatePotentialEnergy(x.Position, x.Velocity));

            @out.WriteLine($"Sum of total energy: {total}");
        }

        public void PartTwo(string[] input, TextWriter @out)
        {
            var points = ParsePoints(input);

            var step = Simulator.SimulateUntilLoop(points);

            @out.WriteLine($"Universe loops at step: {step}");
        }

        public static int CalculatePotentialEnergy(Point3d position, Point3d velocity)
        {
            var potential = Math.Abs(position.X) + Math.Abs(position.Y) + Math.Abs(position.Z);
            var kinetic = Math.Abs(velocity.X) + Math.Abs(velocity.Y) + Math.Abs(velocity.Z);

            return potential * kinetic;
        }

        public static ImmutableList<Point3d> ParsePoints(string[] input) => input.Select(ParsePoint).ToImmutableList();

        private static readonly Regex _regex = new Regex(@"<x=(?<x>-?\d+), y=(?<y>-?\d+), z=(?<z>-?\d+)>", RegexOptions.Compiled);

        public static Point3d ParsePoint(string input)
        {
            var res = _regex.Match(input);

            return new Point3d(
                int.Parse(res.Groups["x"].Value),
                int.Parse(res.Groups["y"].Value),
                int.Parse(res.Groups["z"].Value)
            );
        }
    }
}
