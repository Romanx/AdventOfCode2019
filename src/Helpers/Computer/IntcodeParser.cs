using System.Collections.Immutable;
using System.Linq;

namespace Helpers.Computer
{
    public static class IntcodeParser
    {
        public static ImmutableArray<long> Parse(string input) => input.Split(',').Select(long.Parse).ToImmutableArray();
    }
}
