using Helpers.Instructions;
using System;

namespace Helpers.Computer.Instructions
{
    internal class LessThanInstruction : Instruction
    {
        public override int OpCode => 7;

        public override ParameterType[] Parameters => new ParameterType[]
        {
            ParameterType.Read,
            ParameterType.Read,
            ParameterType.Write
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameterModes, ref Span<int> memory, Runtime runtime)
        {
            var instructionParameters = GetParameters(index, parameterModes, memory);

            var first = instructionParameters[0];
            var second = instructionParameters[1];
            var outAddress = instructionParameters[2];

            memory[outAddress] = first < second
                ? 1
                : 0;

            index += 4;
        }
    }
}
