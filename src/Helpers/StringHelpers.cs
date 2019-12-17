using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class StringHelpers
    {
        public static IEnumerable<int> CharsToDigit(this string @in) => @in.Select(c => c - '0');
    }
}
