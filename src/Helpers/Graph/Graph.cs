using System.Collections.Generic;
using System.Collections.Immutable;

namespace Helpers.Graph
{
    public class Graph
    {
        private readonly Dictionary<string, List<string>> _adjacentList = new Dictionary<string, List<string>>();

        public ImmutableDictionary<string, ImmutableArray<string>> AdjacentList
            => _adjacentList.ToImmutableDictionary(k => k.Key, v => v.Value.ToImmutableArray());

        public void AddVertex(string id)
        {
            if (_adjacentList.ContainsKey(id))
                return;

            _adjacentList.Add(id, new List<string>());
        }

        public void AddEdge(string aId, string bId)
        {
            AddVertex(aId);
            _adjacentList[aId].Add(bId);

            AddVertex(bId);
            _adjacentList[bId].Add(aId);
        }
    }
}
