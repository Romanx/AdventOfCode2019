using System;

namespace Helpers.Instructions
{
    internal class AddInstruction : Instruction
    {
        public override OpCodes OpCode => OpCodes.Add;

        public override ParameterType[] Parameters { get; } = new[]
        {
            ParameterType.Read,
            ParameterType.Read,
            ParameterType.Write
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameters, IntcodeComputer runtime, ref int[] memory)
        {
            memory[parameters[2]] = parameters[0] + parameters[1];

            index += 4;
        }
    }
}

