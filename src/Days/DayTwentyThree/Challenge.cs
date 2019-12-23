using Helpers;
using Helpers.Computer;
using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DayTwentyThree
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 23)
        {
        }

        public override string Name => "Category Six";

        public void PartOne(string input, TextWriter @out)
        {
            var program = IntcodeParser.Parse(input);
            var network = new Network(50, program, @out);

            var result = network.Simulate();

            @out.WriteLine($"Packet Sent to 255: X = {result.X}, Y = {result.Y}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var program = IntcodeParser.Parse(input);
            var network = new Network(50, program, @out);

            var result = network.SimulateWithNat();

            @out.WriteLine($"Packet Sent to 0 Twice: Y = {result.Y}");
        }
    }
}
