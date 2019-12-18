using Helpers.Points;
using System.Collections.Immutable;

namespace DaySeventeen
{
    public static class Parser
    {
        public static ImmutableDictionary<Point, char> Parse(string[] lines)
        {
            var dict = ImmutableDictionary.CreateBuilder<Point, char>();

            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    dict.Add((x, y), line[x]);
                }
            }

            return dict.ToImmutable();
        }
    }
}
