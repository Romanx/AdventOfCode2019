using System;
using System.Collections.Generic;
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
    }

    public static class ShortestPath
    {
        public static string[] Search(Graph graph, string start, string end)
        {
            var shortestFunc = BuildShortestFunction(graph, start);

            return shortestFunc(end);
        }

        private static Func<string, string[]> BuildShortestFunction(Graph graph, string start)
        {
            var previous = new Dictionary<string, string>();

            var queue = new Queue<string>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                
                foreach (var neighbour in graph.AdjacentList[vertex])
                {
                    if (previous.ContainsKey(neighbour))
                        continue;

                    previous[neighbour] = vertex;
                    queue.Enqueue(neighbour);
                }
            }

            return (target) =>
            {
                var path = new List<string>();

                var current = target;
                while (current != start)
                {
                    path.Add(current);
                    current = previous[current];
                }

                path.Add(start);

                path.Reverse();
                return path.ToArray();
            };
        }
    }
}
