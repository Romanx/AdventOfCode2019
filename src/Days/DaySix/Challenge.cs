using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DaySix
{
    public class Challenge : ChallengeBase, INeedLines
    {
        private const string CenterOfMass = "COM";

        public Challenge() : base(day: 6)
        {
        }

        public override string Name => "Universal Orbit Map";

        public void PartOne(string[] lines, TextWriter @out)
        {
            var nodes = ParseSimple(lines);

            var total = nodes.Keys
                .Sum(n => GetOrbitCount(n, nodes));

            @out.WriteLine($"Total Orbits : {total}");
        }

        public void PartTwo(string[] lines, TextWriter @out)
        {
            var nodes = ParseSimple(lines);

            var youPath = FindPathToCom(nodes, "YOU");
            var sanPath = FindPathToCom(nodes, "SAN");

            var first = youPath.Intersect(sanPath).First();

            var i = Array.IndexOf(youPath, first);
            var j = Array.IndexOf(sanPath, first);

            @out.WriteLine($"Number of transfers required: {i + j}");
        }

        private Dictionary<string, string> ParseSimple(string[] lines)
        {
            var dict = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                var span = line.AsSpan();
                var parent = span[..line.IndexOf(')')].ToString();
                var child = span[(line.IndexOf(')') + 1)..].ToString();

                dict.Add(child, parent);
            }

            return dict;
        }

        private string[] FindPathToCom(Dictionary<string, string> nodes, string start)
        {
            var path = new List<string>();
            var node = start;
            while (node != CenterOfMass)
            {
                node = nodes[node];
                path.Add(node);
            }

            return path.ToArray();
        }

        private int GetOrbitCount(string node, Dictionary<string, string> nodes)
        {
            if (node == CenterOfMass)
                return 0;

            return 1 + GetOrbitCount(nodes[node], nodes);
        }
    }
}
