using Helpers.Instructions;
using System;

namespace Helpers.Computer.Instructions
{
    internal class ReadInstruction : Instruction
    {
        public override int OpCode => 3;

        public override ParameterType[] Parameters { get; } = new[]
        {
            ParameterType.Write
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameterModes, ref Span<int> memory, Runtime runtime)
        {
            var outputAddress = memory[index + 1];

            var input = runtime.Input.Pop();

            memory[outputAddress] = input;

            index += 2;
        }
    }
}
