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
                .OrderByDescending(point => CountVisibleAsteroids(map, point))
                .First();

            @out.WriteLine($"Best Asteroid is: {best} with {CountVisibleAsteroids(map, best)} visible");
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
            var sorted = new SortedDictionary<double, Queue<Point>>(map.Points.Where(p => p != station)
                .GroupBy(o => AngleInDegrees(station, o))
                .ToDictionary(k => k.Key, v => new Queue<Point>(v.OrderBy(x => ManhattanDistance(x, station)))));

            var banged = ImmutableArray.CreateBuilder<Point>(map.Points.Length);

            while (sorted.Count > 0)
            {
                foreach (var kvp in sorted)
                {
                    var asteroids = kvp.Value;
                    var asteroid = asteroids.Dequeue();

                    banged.Add(asteroid);
                }

                sorted = new SortedDictionary<double, Queue<Point>>(sorted
                    .Where(pair => pair.Value.Count > 0)
                    .ToDictionary(pair => pair.Key, pair => pair.Value));
            }

            return banged.ToImmutable();
        }
    }
}
