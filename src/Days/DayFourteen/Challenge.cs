using Helpers;
using System;
using System.IO;
using System.Linq;

namespace DayFourteen
{
    public class Challenge : ChallengeBase, INeedLines
    {
        public Challenge() : base(day: 14)
        {
        }

        public override string Name => "Space Stoichiometry";

        public void PartOne(string[] input, TextWriter @out)
        {
            var parsed = Parser.Parse(input);
            var factory = new Factory(parsed);

            var ore = factory.CalculateOreCost(1);

            @out.WriteLine($"Ore Total for Fuel: {ore}");
        }

        public void PartTwo(string[] input, TextWriter @out)
        {
            var parsed = Parser.Parse(input);
            const long target = 1_000_000_000_000;
            bool narrowing = false;
            long fuelInput = 1;
            long fuelJump = 1;

            while (true)
            {
                var factory = new Factory(parsed);
                @out.WriteLine($"Fuel input currently is: {fuelInput} with jump {fuelJump}");
                var result = factory.CalculateOreCost(fuelInput);

                if (result > target)
                {
                    narrowing = true;
                    fuelJump = Math.Max(1, fuelJump / 2);
                    fuelInput -= fuelJump;
                }
                else if (!narrowing)
                {
                    fuelJump *= 2;
                    fuelInput += fuelJump;
                }
                else
                {
                    if (fuelJump == 1)
                        break;

                    fuelInput += fuelJump;
                }
            }

            @out.WriteLine($"Found Target Fuel: {fuelInput}");
        }
    }
}
