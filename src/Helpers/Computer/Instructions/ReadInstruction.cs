using Helpers.Instructions;
using System;

namespace Helpers.Computer.Instructions
{
    internal class ReadInstruction : Instruction
    {
        public override OpCodes OpCode => OpCodes.Read;

        public override ParameterType[] Parameters { get; } = new[]
        {
            ParameterType.Write
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameters, IntcodeComputer computer, ref int[] memory)
        {
            var outputAddress = memory[index + 1];

            var input = computer.Input.Dequeue();

            memory[outputAddress] = input;

            index += 2;
        }
    }
}
