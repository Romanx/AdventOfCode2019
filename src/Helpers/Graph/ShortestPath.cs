using System;
using System.Collections.Generic;

namespace Helpers.Graph
{
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
