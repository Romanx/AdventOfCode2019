using Helpers.Instructions;
using System;

namespace Helpers.Computer.Instructions
{
    internal class JumpIfTrueInstruction : Instruction
    {
        public override OpCodes OpCode => OpCodes.JumpIfTrue;

        public override ParameterType[] Parameters => new ParameterType[]
        {
            ParameterType.Read,
            ParameterType.Read
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameters, IntcodeComputer runtime, ref int[] memory)
        {
            if (parameters[0] != 0)
            {
                index = parameters[1];
            }
            else
            {
                index += 3;
            }
        }
    }
}
