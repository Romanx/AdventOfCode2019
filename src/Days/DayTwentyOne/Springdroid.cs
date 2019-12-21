using Helpers;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayTwentyOne
{
    public class Springdroid
    {
        private readonly ImmutableArray<long> _program;

        public Springdroid(ImmutableArray<long> program)
        {
            _program = program;
        }

        public long Run(ImmutableArray<Instruction> instructions, TextWriter? writer = null)
        {
            var computer = new IntcodeComputer(_program);
            WriteInstructions(computer, instructions);

            var result = computer.Run();

            if (result != IntcodeResult.HALT_TERMINATE)
            {
                throw new InvalidOperationException("Error!");
            }

            writer?.WriteLine(new string(computer.Output.Select(l => (char)l).ToArray()));

            return computer.Output.Last();
        }

        private static void WriteCommand(IntcodeComputer computer, string command)
        {
            foreach (var c in command.ToCharArray().Select(x => (int)x))
            {
                computer.Input.Enqueue(c);
            }
            computer.Input.Enqueue(10);
        }

        private static void WriteInstructions(IntcodeComputer computer, ImmutableArray<Instruction> instructions)
        {
            foreach (var instruction in instructions.Select(i => i.Command))
            {
                WriteCommand(computer, instruction);
            }
        }
    }
}
