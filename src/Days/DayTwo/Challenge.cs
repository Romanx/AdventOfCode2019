using Helpers;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayTwo
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 2)
        {
        }

        public override string Name => "1202 Program Alarm";

        public void PartOne(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(int.Parse).ToArray();

            memory[1] = 12;
            memory[2] = 2;

            var computer = new IntcodeComputer(memory.ToImmutableArray());
            computer.Run();
            var output = computer.Output.Dequeue();

            @out.WriteLine($"Computer result: {output}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(int.Parse).ToImmutableArray();

            var scratch = new int[memory.Length];

            foreach (var noun in Enumerable.Range(0, 100))
            {
                foreach (var verb in Enumerable.Range(0, 100))
                {
                    memory.CopyTo(scratch);

                    scratch[1] = noun;
                    scratch[2] = verb;

                    var computer = new IntcodeComputer(scratch.ToImmutableArray());
                    computer.Run();
                    var result = computer.Output.Dequeue();

                    if (result == 19690720)
                    {
                        @out.WriteLine($"Noun: {noun}, Verb: {verb}");
                        @out.WriteLine($"Result: 100 * {noun} + {verb} = {100 * noun + verb}");
                        goto end;
                    }
                }
            }
            end:
            @out.WriteLine("Done");
        }
    }
}
