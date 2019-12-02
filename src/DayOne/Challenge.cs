using Helpers;
using System;
using System.IO;
using System.Linq;

namespace DayOne
{
    public class Challenge : ChallengeBase, INeedLines
    {
        public Challenge() : base(day: 1)
        {
        }

        public void PartOne(string[] lines, TextWriter @out)
        {
            var totalMass = lines
                .Select(int.Parse)
                .Select(CalculateFuel)
                .Sum();

            @out.WriteLine($"Total Mass: {totalMass:N0} ({totalMass})");
        }

        public void PartTwo(string[] lines, TextWriter @out)
        {
            var totalMass = lines
                .Select(int.Parse)
                .Select(AggregateFuel)
                .Sum();

            @out.WriteLine($"Total Mass: {totalMass:N0} ({totalMass})");
        }

        private long AggregateFuel(int mass)
        {
            var total = CalculateFuel(mass);
            return total <= 0
                ? 0L
                : total + AggregateFuel(total);
        }

        private int CalculateFuel(int mass) => ((int)Math.Floor(mass / 3m)) - 2;
    }
}
