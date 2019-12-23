using System.Text.RegularExpressions;

namespace DayTwentyTwo
{
    public static class RegexExtensions
    {
        public static bool TryMatch(this Regex regex, string input, out string match)
        {
            var m = regex.Match(input);
            match = string.Empty;
            if (m.Success)
            {
                match = m.Groups[1].Value;
            }

            return m.Success;
        }
    }
}
