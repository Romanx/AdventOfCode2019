using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace DayFourteen
{
    public class Factory
    {
        private const string OreConstant = "ORE";
        private const string FuelConstant = "FUEL";
        private readonly Dictionary<string, long> _resources = new Dictionary<string, long>();
        private readonly ImmutableArray<Recipe> _recipes;

        public Dictionary<string, long> Needed { get; } = new Dictionary<string, long>();

        public Factory(ImmutableArray<Recipe> recipes)
        {
            _recipes = recipes;
        }

        public long CalculateOreCost(long fuelAmount)
        {
            Needed[FuelConstant] = fuelAmount;
            Produce(FuelConstant);
            return Needed[OreConstant];
        }

        public void Produce(string name)
        {
            var formula = _recipes.First(x => x.Output.Type == name);
            var output = formula.Output;
            var count = (long)Math.Ceiling(
                Math.Max(0, Needed[output.Type] - GetValueOrDefault(_resources, output.Type)) / Convert.ToDecimal(output.Quantity));

            AddOrIncrement(_resources, output.Type, count * output.Quantity);

            foreach (var input in formula.Inputs)
            {
                AddOrIncrement(Needed, input.Type, count * input.Quantity);
            }

            foreach (var input in formula.Inputs)
            {
                if (input.Type != OreConstant)
                {
                    Produce(input.Type);
                }
            }

            static long GetValueOrDefault(Dictionary<string, long> dict, string type)
            {
                return dict.TryGetValue(type, out var res) ? res : 0;
            }

            static void AddOrIncrement(Dictionary<string, long> dict, string type, long value)
            {
                if (dict.ContainsKey(type))
                {
                    dict[type] += value;
                }
                else
                {
                    dict[type] = value;
                }
            }
        }
    }
}
