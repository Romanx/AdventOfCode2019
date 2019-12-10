using Helpers.Points;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DayTen
{
    public class Map
    {
        private Map(ImmutableArray<Point> points)
        {
            Points = points;
        }

        public ImmutableArray<Point> Points { get; }

        public static Map Parse(string[] lines)
        {
            var points = new List<Point>();

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        points.Add(new Point(x, y));
                }
            }

            return new Map(points.ToImmutableArray());
        }
    }
}
