using Helpers;
using Helpers.Computer;
using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayFifteen
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 15)
        {
        }

        public override string Name => "Oxygen System";

        public void PartOne(string input, TextWriter @out)
        {
            var program = IntcodeParser.Parse(input);
            var droid = new Droid(program);
            var map = droid.Run();
            PrintMap(map, @out);

            var func = BuildShortestFunction(map, Point.Origin);
            var target = map.First(x => x.Value == CellType.OxygenSystem).Key;

            var path = func(target);
            @out.WriteLine($"Found path the oxygen system with length {path.Length - 1}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var program = IntcodeParser.Parse(input);
            var droid = new Droid(program);
            var map = droid.Run();

            var source = map.First(x => x.Value == CellType.OxygenSystem).Key;

            var filled = Filler.FloodFill(map, source);
            var steps = filled.Values.Max();

            @out.WriteLine($"Number of minutes taken to spread: {steps}");
        }

        private static Func<Point, ImmutableArray<Point>> BuildShortestFunction(ImmutableDictionary<Point, CellType> map, Point start)
        {
            var previous = new Dictionary<Point, Point>();

            var queue = new Queue<Point>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var point = queue.Dequeue();

                foreach (var neighbour in AjacentPoints(point))
                {
                    if (previous.ContainsKey(neighbour))
                        continue;

                    previous[neighbour] = point;
                    queue.Enqueue(neighbour);
                }
            }

            return (target) =>
            {
                var path = new List<Point>();

                var current = target;
                while (current != start)
                {
                    path.Add(current);
                    current = previous[current];
                }

                path.Add(start);

                path.Reverse();
                return path.ToImmutableArray();
            };

            IEnumerable<Point> AjacentPoints(Point point)
            {
                if (IsNotWall(point + (0, 1))) yield return point + (0, 1);
                if (IsNotWall(point + (0, -1))) yield return point + (0, -1);
                if (IsNotWall(point + (1, 0))) yield return point + (1, 0);
                if (IsNotWall(point + (-1, 0))) yield return point + (-1, 0);
            }

            bool IsNotWall(Point point) => map.TryGetValue(point, out var type) && type != CellType.Wall;
        }

        public static void PrintMap(ImmutableDictionary<Point, CellType> map, TextWriter @out)
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
                        switch (type)
                        {
                            case CellType.Empty:
                                @out.Write(".");
                                break;
                            case CellType.Wall:
                                @out.Write("#");
                                break;
                            case CellType.OxygenSystem:
                                @out.Write("S");
                                break;
                        }
                    }
                    else
                    {
                        @out.Write(" ");
                    }
                }
                @out.WriteLine();
            }
        }
    }
}
