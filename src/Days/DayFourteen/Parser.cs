using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace DayFourteen
{
    public static class Parser
    {
        private static readonly Regex _recipeRegex = new Regex(@"(\d+) (.*)");

        public static ImmutableArray<Recipe> Parse(string[] input) => input.Select(ParseRecipe).ToImmutableArray();

        private static Recipe ParseRecipe(string input)
        {
            var split = input.Split("=>");
            var inputs = split[0].Split(',').Select(ToComponent);
            var output = ToComponent(split[1]);

            return new Recipe(inputs, output);

            static (int Quantity, string Type) ToComponent(string @in)
            {
                var match = _recipeRegex.Match(@in.Trim());

                return (int.Parse(match.Groups[1].Value), match.Groups[2].Value);
            }
        }
    }
}
