using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Helpers;

namespace DayOne
{
    public class DayOneChallenge : ChallengeBase
    {
        public DayOneChallenge() : base (day: 1)
        {
        }

        public override async Task PartOne(string[] lines, TextWriter @out)
        {
            var totalMass = lines
                .Select(long.Parse)
                .Select(CalculateFuel)
                .Sum();

            await @out.WriteLineAsync($"Total Mass: {totalMass:N0} ({totalMass})");
        }

        public override async Task PartTwo(string[] lines, TextWriter @out)
        {
            var totalMass = lines
                .Select(long.Parse)
                .Select(AggregateFuel)
                .Sum();

            await @out.WriteLineAsync($"Total Mass: {totalMass:N0} ({totalMass})");
        }

        private long AggregateFuel(long mass)
        {
            var total = CalculateFuel(mass);
            return total <= 0 
                ? 0L 
                : total + AggregateFuel(total);
        }

        private long CalculateFuel(long mass) => ((int)Math.Floor(mass / 3m)) - 2;
    }
}
