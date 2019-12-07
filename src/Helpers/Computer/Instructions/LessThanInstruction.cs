using Helpers.Instructions;
using System;

namespace Helpers.Computer.Instructions
{
    internal class LessThanInstruction : Instruction
    {
        public override OpCodes OpCode => OpCodes.LessThan;

        public override ParameterType[] Parameters => new ParameterType[]
        {
            ParameterType.Read,
            ParameterType.Read,
            ParameterType.Write
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameters, IntcodeComputer runtime, ref int[] memory)
        {
            var first = parameters[0];
            var second = parameters[1];
            var outAddress = parameters[2];

            memory[outAddress] = first < second
                ? 1
                : 0;

            index += 4;
        }
    }
}
