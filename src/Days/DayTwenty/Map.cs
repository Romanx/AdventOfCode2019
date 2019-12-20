using Helpers;
using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DayTwenty
{
    public class Map
    {
        private readonly ImmutableDictionary<Point, Entity> _mapArr;

        public Map(ImmutableDictionary<Point, Entity> immutableDictionary)
        {
            _mapArr = immutableDictionary;
        }

        public (Point Position, Entity Entity) FindPortal(string identifier)
        {
            var (pos, entity) = _mapArr.First(x => x.Value.Identifier.Equals(identifier, StringComparison.OrdinalIgnoreCase));
            return (pos, entity);
        }

        public int? FindDistanceWithRecursion(Point start, Point end)
        {
            var queue = new Queue<(Point Point, int Distance, int Level)>();
            var visited = new HashSet<(Point, int)>();

            queue.Enqueue((start, 0, 0));

            while (queue.Count > 0)
            {
                var (point, distance, level) = queue.Dequeue();

                if (point == end && level == 0)
                {
                    return distance;
                }

                if (visited.Contains((point, level)))
                {
                    continue;
                }

                visited.Add((point, level));

                foreach (var (neighbour, resLevel) in AdjacentFunction(point, level))
                {
                    queue.Enqueue((neighbour, distance + 1, resLevel));
                }
            }

            return null;

            IEnumerable<(Point Point, int Level)> AdjacentFunction(Point point, int level)
            {
                var entity = _mapArr[point];
                if (entity is PortalEntity pe)
                {
                    var (matchingPortal, _) = _mapArr.FirstOrDefault(x => x.Value.Identifier == entity.Identifier && x.Key != point);

                    if (matchingPortal is null)
                    {
                        return GetAdjacentNonWalls(point, level);
                    }

                    var newLevel = pe.PortalType == PortalType.Outer
                        ? level - 1
                        : level + 1;

                    return GetAdjacentNonWalls(point, level).Concat(new[] { (matchingPortal, newLevel) });
                }

                return GetAdjacentNonWalls(point, level);
            }

            IEnumerable<(Point, int)> GetAdjacentNonWalls(Point point, int level)
            {
                var e = GetAdjacent(_mapArr, point, (p, e) =>
                {
                    if (level == 0 && e is PortalEntity pe && pe.PortalType == PortalType.Outer)
                    {
                        if (p == start || p == end)
                        {
                            return true;
                        }

                        return false;
                    }

                    if (level > 0 && e is PortalEntity pe2 && pe2.PortalType == PortalType.Outer)
                    {
                        if (p == start || p == end)
                        {
                            return false;
                        }

                        return true;
                    }

                    return e.Type != EntityType.Wall;
                });
                return e.Select(e => (e.Position, level));
            }
        }

        public int? FindDistance(Point start, Point end)
        {
            var queue = new Queue<(Point Point, int Distance)>();
            var visited = new HashSet<Point>();

            queue.Enqueue((start, 0));

            while (queue.Count > 0)
            {
                var (point, distance) = queue.Dequeue();

                if (point == end)
                {
                    return distance;
                }

                if (visited.Contains(point))
                {
                    continue;
                }

                visited.Add(point);

                foreach (var neighbour in AdjacentFunction(point))
                {
                    queue.Enqueue((neighbour, distance + 1));
                }
            }

            return null;

            IEnumerable<Point> AdjacentFunction(Point point)
            {
                var entity = _mapArr[point];
                if (entity.Type == EntityType.Portal)
                {
                    var (matchingPortal, _) = _mapArr.FirstOrDefault(x => x.Value.Identifier == entity.Identifier && x.Key != point);

                    if (matchingPortal is null)
                    {
                        return GetAdjacentNonWalls(point);
                    }

                    return GetAdjacentNonWalls(point).Concat(new[] { matchingPortal });
                }

                return GetAdjacentNonWalls(point);
            }

            IEnumerable<Point> GetAdjacentNonWalls(Point point) => GetAdjacent(_mapArr, point, (_, e) => e.Type != EntityType.Wall)
                    .Select(e => e.Position);
        }

        public static IEnumerable<(Point Position, Entity Entity)> GetAdjacent(IDictionary<Point, Entity> map, Point point, Func<Point, Entity, bool>? filter = null)
        {
            filter ??= (p, e) => true;

            var northPoint = point + Direction.North;
            var eastPoint = point + Direction.East;
            var southPoint = point + Direction.South;
            var westPoint = point + Direction.West;

            if (map.TryGetValue(northPoint, out var northEntity) && filter(northPoint, northEntity)) yield return (northPoint, northEntity);
            if (map.TryGetValue(eastPoint, out var eastEntity) && filter(eastPoint, eastEntity)) yield return (eastPoint, eastEntity);
            if (map.TryGetValue(southPoint, out var southEntity) && filter(southPoint, southEntity)) yield return (southPoint, southEntity);
            if (map.TryGetValue(westPoint, out var westEntity) && filter(westPoint, westEntity)) yield return (westPoint, westEntity);
        }
    }
}
