using Helpers.Points;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DayTwenty
{
    public static class Parser
    {
        public static Map Parse(string[] lines)
        {
            var builder = ImmutableDictionary.CreateBuilder<Point, Entity>();

            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var isOuter = x < 2
                        || x >= line.Length - 2
                        || y < 2
                        || y >= lines.Length - 2;

                    builder[(x, y)] = line[x] switch 
                    {
                        '#' => Entity.Wall,
                        '.' => Entity.Empty,
                        ' ' => Entity.Wall,
                        char c when char.IsUpper(c) => new PortalEntity(
                            c.ToString(),
                            isOuter ? PortalType.Outer : PortalType.Inner),
                        _ => throw new InvalidOperationException("Invalid entity type"),
                    };
                }
            }

            SquashPortals(builder);

            return new Map(builder.ToImmutable());
        }

        private static void SquashPortals(ImmutableDictionary<Point, Entity>.Builder map)
        {
            var ajacentEmpties = map
                .Where(x => x.Value.Type == EntityType.Empty && Map.GetAdjacent(map, x.Key).Any(a => a.Entity.Type == EntityType.Portal))
                .ToArray();

            foreach (var (point, entity) in ajacentEmpties)
            {
                var aPortal = Map.GetAdjacent(map, point, (_, e) => e.Type == EntityType.Portal).First();
                var bPortal = Map.GetAdjacent(map, aPortal.Position, (_, e) => e.Type == EntityType.Portal).First();
                map.Remove(aPortal.Position);
                map.Remove(bPortal.Position);
                map[point] = MergePortals((aPortal.Position, (PortalEntity)aPortal.Entity), (bPortal.Position, (PortalEntity)bPortal.Entity));
            }
        }

        private static Entity MergePortals((Point Point, PortalEntity Entity) a, (Point Point, PortalEntity Entity) b)
        {
            if (a.Entity.PortalType != b.Entity.PortalType)
                throw new InvalidOperationException("Portal types must match");

            var identifier = a switch
            {
                _ when a.Point.X > b.Point.X => $"{b.Entity.Identifier}{a.Entity.Identifier}",
                _ when a.Point.X < b.Point.X => $"{a.Entity.Identifier}{b.Entity.Identifier}",
                _ when a.Point.Y > b.Point.Y => $"{b.Entity.Identifier}{a.Entity.Identifier}",
                _ when b.Point.Y > a.Point.Y => $"{a.Entity.Identifier}{b.Entity.Identifier}",
                _ => throw new InvalidOperationException("Didn't handle merge correctly")
            };

            return new PortalEntity(identifier, a.Entity.PortalType);
        }
    }
}
