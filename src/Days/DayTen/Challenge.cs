using Helpers;
using Helpers.Points;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using static Helpers.Points.PointHelpers;

namespace DayTen
{
    public class Challenge : ChallengeBase, INeedLines
    {
        public Challenge() : base(day: 10)
        {
        }

        public override string Name => "Monitoring Station";

        public void PartOne(string[] input, TextWriter @out)
        {
            var map = Map.Parse(input);

            var best = map.Points
                .Select(p => (Point: p, Visible: CountVisibleAsteroids(map, p)))
                .OrderByDescending(i => i.Visible)
                .First();

            @out.WriteLine($"Best Asteroid is: {best.Point} with {best.Visible} visible");
        }

        public void PartTwo(string[] input, TextWriter @out)
        {
            var map = Map.Parse(input);
            var station = map.Points
                .OrderByDescending(point => CountVisibleAsteroids(map, point))
                .First();

            var banged = DestroyedOrderList(map, station);

            for (var i = 0; i < banged.Length; i++)
            {
                @out.WriteLine($"{i + 1}: Bang! - {banged[i]}");
            }

            var targetAsteriod = banged[199];

            @out.WriteLine($"Number 200 was: {targetAsteriod}");
            @out.WriteLine($"Result is: {(targetAsteriod.X * 100) + targetAsteriod.Y}");
        }

        private int CountVisibleAsteroids(Map map, Point point)
        {
            return map.Points.Where(p => p != point)
                .GroupBy(o => AngleInRadians(point, o))
                .Count();
        }

        private ImmutableArray<Point> DestroyedOrderList(Map map, Point station)
        {
            var sorted = map.Points.Where(p => p != station)
                .GroupBy(o => AngleInDegrees(station, o))
                .Select(g => (Angle: g.Key, Queue: new Queue<Point>(g.OrderBy(x => ManhattanDistance(x, station)))))
                .OrderBy(x => x.Angle)
                .ToImmutableList();

            var banged = ImmutableArray.CreateBuilder<Point>(map.Points.Length);

            while (sorted.Count > 0)
            {
                foreach (var (_, asteroids) in sorted)
                {
                    var asteroid = asteroids.Dequeue();

                    banged.Add(asteroid);
                }

                sorted = sorted.RemoveAll(i => i.Queue.Count == 0);
            }

            return banged.ToImmutable();
        }
    }
}
