using Helpers.Instructions;
using System;

namespace Helpers.Computer.Instructions
{
    internal class JumpIfFalseInstruction : Instruction
    {
        public override int OpCode => 6;

        public override ParameterType[] Parameters => new ParameterType[]
        {
            ParameterType.Read,
            ParameterType.Read
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameterModes, ref Span<int> memory, Runtime runtime)
        {
            var instructionParameters = GetParameters(index, parameterModes, memory);

            if (instructionParameters[0] == 0)
            {
                index = instructionParameters[1];
            }
            else
            {
                index += 3;
            }
        }
    }
}
