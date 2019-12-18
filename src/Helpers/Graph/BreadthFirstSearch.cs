using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Helpers.Graph
{
    public static class BreadthFirstSearch
    {
        public static HashSet<string> Search(Graph graph, string startingNodeId)
        {
            var visited = new HashSet<string>();

            if (!graph.AdjacentList.ContainsKey(startingNodeId))
                return visited;

            var queue = new Queue<string>();
            queue.Enqueue(startingNodeId);

            while (queue.Count > 0)
            {
                var elementId = queue.Dequeue();

                if (visited.Contains(elementId))
                    continue;

                visited.Add(elementId);

                var adjacent = graph.AdjacentList[elementId];

                foreach (var id in adjacent.Where(i => !visited.Contains(i)))
                {
                    queue.Enqueue(id);
                }
            }

            return visited;
        }

        public static ImmutableArray<Point> Search(Point start, Func<Point, IEnumerable<Point>> adjacentFunction)
        {
            var visited = ImmutableArray.CreateBuilder<Point>();

            var queue = new Queue<Point>();
            queue.Enqueue(start);

            while (queue.TryDequeue(out var node))
            {
                if (visited.Contains(node))
                    continue;

                visited.Add(node);

                var adjacentNodes = adjacentFunction(node);

                foreach (var adjacentNode in adjacentNodes)
                {
                    if (!visited.Contains(adjacentNode))
                    {
                        queue.Enqueue(adjacentNode);
                    }
                }
            }

            return visited.ToImmutable();
        }
    }
}
