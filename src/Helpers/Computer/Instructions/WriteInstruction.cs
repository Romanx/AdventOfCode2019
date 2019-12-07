using Helpers.Instructions;
using System;

namespace Helpers.Computer.Instructions
{
    internal class WriteInstruction : Instruction
    {
        public override OpCodes OpCode => OpCodes.Write;

        public override ParameterType[] Parameters => new []
        {
            ParameterType.Read
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameters, IntcodeComputer runtime, ref int[] memory)
        {
            runtime.Output.Enqueue(parameters[0]);

            index += 2;
        }
    }
}
