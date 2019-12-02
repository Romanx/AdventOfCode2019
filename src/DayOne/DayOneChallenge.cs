using Helpers;
using System;
using System.IO;
using System.Linq;

namespace DayOne
{
    public class DayOneChallenge : ChallengeBase, INeedLines
    {
        public DayOneChallenge() : base(day: 1)
        {
        }

        public void PartOne(string[] lines, TextWriter @out)
        {
            var totalMass = lines
                .Select(long.Parse)
                .Select(CalculateFuel)
                .Sum();

            @out.WriteLine($"Total Mass: {totalMass:N0} ({totalMass})");
        }

        public void PartTwo(string[] lines, TextWriter @out)
        {
            var totalMass = lines
                .Select(long.Parse)
                .Select(AggregateFuel)
                .Sum();

            @out.WriteLine($"Total Mass: {totalMass:N0} ({totalMass})");
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
