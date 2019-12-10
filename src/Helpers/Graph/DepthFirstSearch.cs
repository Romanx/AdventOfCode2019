using System;
using System.Collections.Generic;

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
    }
}
