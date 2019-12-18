using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Helpers.Graph
{
    public static class DepthFirstSearch
    {
        public static HashSet<string> Search(Graph graph, string startingNodeId, Action<string>? action = null)
        {
            var visited = new HashSet<string>();

            InnerSearch(startingNodeId);

            return visited;

            void InnerSearch(string elementId)
            {
                visited.Add(elementId);
                action?.Invoke(elementId);

                var adjacent = graph.AdjacentList[elementId];

                foreach (var id in adjacent)
                {
                    if (!visited.Contains(id))
                    {
                        InnerSearch(id);
                    }
                }
            }
        }

        public static ImmutableArray<Point> Search(Point start, Func<Point, IEnumerable<Point>> adjacentFunction)
        {
            var visited = ImmutableArray.CreateBuilder<Point>();

            InnerSearch(start);

            return visited.ToImmutable();

            void InnerSearch(Point node)
            {
                visited.Add(node);

                var adjacentNodes = adjacentFunction(node);

                foreach (var id in adjacentNodes)
                {
                    if (!visited.Contains(id))
                    {
                        InnerSearch(id);
                    }
                }
            }
        }
    }
}
