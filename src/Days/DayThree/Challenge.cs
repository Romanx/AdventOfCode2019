using Helpers;
using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayThree
{
    public class Challenge : ChallengeBase, INeedLines
    {
        private static readonly Point CentralPort = new Point(0, 0);

        public Challenge() : base(day: 3)
        {
        }

        public override string Name => "Crossed Wires";

        public void PartOne(string[] lines, TextWriter @out)
        {
            var mappedLines = lines.Select(ParseLine).ToArray();
            var closest = GetLineIntersections(mappedLines)
                .OrderBy(x => PointHelpers.ManhattanDistance(CentralPort, x))
                .First();

            var distance = PointHelpers.ManhattanDistance(closest, CentralPort);

            @out.WriteLine($"Closest point to CentralPort: {closest}: Distance {distance}");
        }

        public void PartTwo(string[] lines, TextWriter @out)
        {
            var mappedLines = lines.Select(ParseLine).ToArray();
            var sumOfSteps = GetIntersectionSteps(mappedLines)
                .OrderBy(k => k)
                .First();

            @out.WriteLine($"{sumOfSteps} Steps");
        }

        public ImmutableHashSet<Point> GetLineIntersections(ImmutableArray<Point>[] lines)
        {
            var results = ImmutableHashSet.CreateBuilder<Point>();
            for (var i = 0; i < lines.Length - 1; i++)
            {
                var line = lines[i];
                var rest = lines[(i + 1)..^0];

                foreach (var other in rest)
                {
                    results.UnionWith(line.Intersect(other));
                }
            }

            return results.ToImmutable();
        }

        public ImmutableArray<int> GetIntersectionSteps(ImmutableArray<Point>[] lines)
        {
            var results = ImmutableArray.CreateBuilder<int>();
            for (var i = 0; i < lines.Length - 1; i++)
            {
                var line = lines[i];
                var rest = lines[(i + 1)..^0];

                foreach (var other in rest)
                {
                    var intersections = line.Intersect(other);
                    foreach (var intersection in intersections)
                    {
                        var idx = line.IndexOf(intersection) + 1;
                        var idx2 = other.IndexOf(intersection) + 1;
                        results.Add(idx + idx2);
                    }
                }
            }

            return results.ToImmutable();
        }

        private ImmutableArray<Point> ParseLine(string line)
        {
            var points = new List<Point>();
            var commands = line.Split(',');
            var p = CentralPort;

            foreach (var command in commands)
            {
                points.AddRange(CommandToPoints(p, command));
                p = points.Last();
            }

            return points.ToImmutableArray();
        }

        private IEnumerable<Point> CommandToPoints(Point start, string command)
        {
            var direction = command[0];
            var length = int.Parse(command[1..^0]);

            return direction switch
            {
                'R' => Enumerable.Range(1, length).Select(i => new Point(start.X + i, start.Y)),
                'L' => Enumerable.Range(1, length).Select(i => new Point(start.X - i, start.Y)),
                'U' => Enumerable.Range(1, length).Select(i => new Point(start.X, start.Y + i)),
                'D' => Enumerable.Range(1, length).Select(i => new Point(start.X, start.Y - i)),
                _ => throw new InvalidOperationException("Direction not valid!"),
            };
        }
    }
}
