using Helpers;
using Helpers.Computer;
using Helpers.Points;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DaySeventeen
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 17)
        {
        }

        public override string Name => "Set and Forget";

        public void PartOne(string input, TextWriter @out)
        {
            var map = ScaffoldingInterface.Run(IntcodeParser.Parse(input));
            PrintMap(map, @out);

            var intersections = FindIntersections(map);

            var alignment = intersections
                .Sum(point => point.X * point.Y);

            @out.WriteLine($"Sum of alignment parameters: {alignment}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var program = IntcodeParser.Parse(input);
            var map = ScaffoldingInterface.Run(program);
            var actions = BuildPath(map);

            var (inputProgram, subsets) = CalculateSubsets(actions);

            @out.WriteLine($"Generated Path: {string.Join(",", actions)}");
            @out.WriteLine($"Generated Program: {string.Join(",", inputProgram)}");
            foreach (var subset in subsets)
            {
                @out.WriteLine($"Subset: {string.Join(",", subset)}");
            }

            var updatedProgram = program.SetItem(0, 2);
            var dust = ScaffoldingInterface.RunWithInput(updatedProgram, inputProgram, subsets);

            @out.WriteLine($"Amount of dust collected by Robot is: {dust}");
        }

        private static ImmutableArray<string> BuildPath(ImmutableDictionary<Point, char> map)
        {
            var output = ImmutableArray.CreateBuilder<string>();
            var robot = map.First(k => k.Value == '^').Key;

            var pos = robot;
            var direction = Direction.North;
            var count = 1;

            while (true)
            {
                var adjacent = GetAdjacentScaffold(pos, map);

                if (adjacent[direction] is Point nextAdjacent)
                {
                    count++;
                    pos = nextAdjacent;
                }
                else if (adjacent[direction.Left()] is Point nextLeft)
                {
                    if (count > 1)
                    {
                        output.AddRange(count.ToString());
                    }

                    output.Add("L");
                    direction = direction.Left();
                    count = 1;
                    pos = nextLeft;
                }
                else if (adjacent[direction.Right()] is Point nextRight)
                {
                    if (count > 1)
                    {
                        output.AddRange(count.ToString());
                    }
                    output.Add("R");
                    direction = direction.Right();
                    count = 1;
                    pos = nextRight;
                }
                else
                {
                    if (count > 1)
                    {
                        output.AddRange(count.ToString());
                    }
                    break;
                }
            }

            return output.ToImmutable();
        }

        private static IEnumerable<Point> FindIntersections(ImmutableDictionary<Point, char> map)
        {
            var scaffolds = map.Where(k => k.Value == '#');

            foreach (var (point, _) in scaffolds)
            {
                if (GetAdjacentScaffold(point, map).Count() == 4)
                {
                    yield return point;
                }
            }
        }

        private static AdjacentPoints GetAdjacentScaffold(Point point, ImmutableDictionary<Point, char> map)
        {
            var north = ScaffoldAtPosition(point + (0, -1), map)
                ? point + (0, -1)
                : null;

            var east = ScaffoldAtPosition(point + (1, 0), map)
                ? point + (1, 0)
                : null;

            var south = ScaffoldAtPosition(point + (0, 1), map)
                ? point + (0, 1)
                : null;

            var west = ScaffoldAtPosition(point + (-1, 0), map)
                ? point + (-1, 0)
                : null;

            return new AdjacentPoints(north, east, south, west);
        }

        private static bool ScaffoldAtPosition(Point point, ImmutableDictionary<Point, char> map) => map.TryGetValue(point, out var type) && type == '#';

        private static void PrintMap(ImmutableDictionary<Point, char> map, TextWriter @out)
        {
            var minX = map.Keys.Min(p => p.X);
            var maxX = map.Keys.Max(p => p.X);
            var minY = map.Keys.Min(p => p.Y);
            var maxY = map.Keys.Max(p => p.Y);

            var xOffset = Math.Abs(minX);
            var yOffset = Math.Abs(minY);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var point = new Point(x, y);

                    if (map.TryGetValue(point, out var type))
                    {
                        @out.Write(type);
                    }
                    else
                    {
                        @out.Write(" ");
                    }
                }
                @out.WriteLine();
            }
        }

        private static (ImmutableArray<string> Program, ImmutableArray<string[]> Subsets) CalculateSubsets(ImmutableArray<string> path, TextWriter? writer = null)
        {
            var random = new Random();
            var outArr = ImmutableArray.CreateBuilder<string[]>(3);

            do
            {
                var scratch = path.ToArray();
                for (var i = 0; i < 3; i++)
                {
                    var len = random.Next(1, 9);
                    var pinnedLength = len > scratch.Length ? scratch.Length : len;
                    var sym = scratch[0..pinnedLength];
                    scratch = scratch.RemoveSequence(sym);
                    outArr.Add(sym);
                }

                var symbols = ConvertToSymbols(path, outArr);
                writer?.WriteLine(symbols);

                if (Regex.IsMatch(symbols, "^[ABC]+$"))
                {
                    break;
                }
                outArr.Clear();
            } while (true);

            var arr = ConvertToSymbols(path, outArr).ToCharArray().Select(c => c.ToString()).ToImmutableArray();

            return (arr, outArr.ToImmutable());
        }

        private static string ConvertToSymbols(ImmutableArray<string> path, ImmutableArray<string[]>.Builder groups)
        {
            var outputStr = path.ToArray();
            for (int i = 0; i < groups.Count; i++)
            {
                var c = (char)(65 + i);
                outputStr = outputStr.ReplaceSequence(groups[i], c.ToString());
            }

            return string.Join("", outputStr);
        }

        private class AdjacentPoints : IEnumerable<Point>
        {
            public AdjacentPoints(Point? north, Point? east, Point? south, Point? west)
            {
                North = north;
                East = east;
                South = south;
                West = west;
            }

            public Point? North { get; }
            public Point? East { get; }
            public Point? South { get; }
            public Point? West { get; }

            public Point? this[Direction dir] => dir.DirectionType switch
            {
                DirectionType.North => North,
                DirectionType.East => East,
                DirectionType.South => South,
                DirectionType.West => West,
                _ => null
            };

            public IEnumerator<Point> GetEnumerator()
            {
                if (North is object) yield return North;
                if (South is object) yield return South;
                if (East is object) yield return East;
                if (West is object) yield return West;
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}
