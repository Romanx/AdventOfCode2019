using Helpers.Points;
using System.Collections.Generic;
using System.Collections.Immutable;
using static DayFifteen.DirectionHelpers;

namespace DayFifteen
{
    public static class Filler
    {
        public static ImmutableDictionary<Point, int> FloodFill(ImmutableDictionary<Point, CellType> map, Point source)
        {
            var queue = new Stack<Point>();
            var dict = ImmutableDictionary.CreateBuilder<Point, int>();
            dict.Add(source, 0);
            queue.Push(source);

            while (queue.Count > 0)
            {
                var node = queue.Pop();

                CheckNodeValid(node, LocationAtDirection(node, Direction.West));
                CheckNodeValid(node, LocationAtDirection(node, Direction.East));
                CheckNodeValid(node, LocationAtDirection(node, Direction.North));
                CheckNodeValid(node, LocationAtDirection(node, Direction.South));
            };

            return dict.ToImmutable();

            void CheckNodeValid(Point parent, Point point)
            {
                if (map.TryGetValue(point, out var type) && type == CellType.Wall)
                    return;

                if (dict.ContainsKey(point))
                    return;

                dict.Add(point, dict[parent] + 1);
                queue.Push(point);
            }
        }
    }
}
