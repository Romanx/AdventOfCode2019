using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DayFourteen
{
    public class Recipe
    {
        public ImmutableArray<(int Quantity, string Type)> Inputs { get; }

        public (int Quantity, string Type) Output { get; }

        public Recipe(IEnumerable<(int Quantity, string Type)> inputs, (int Quantity, string Type) output)
        {
            Inputs = inputs.ToImmutableArray();
            Output = output;
        }

        public override string ToString()
            => $"{string.Join(", ", Inputs.Select(i => $"{i.Quantity} {i.Type}"))} => {Output.Quantity} {Output.Type}";
    }
}
