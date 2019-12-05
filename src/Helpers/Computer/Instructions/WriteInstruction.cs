using Helpers.Instructions;
using System;

namespace Helpers.Computer.Instructions
{
    internal class WriteInstruction : Instruction
    {
        public override int OpCode => 4;

        public override ParameterType[] Parameters => new []
        {
            ParameterType.Read
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameterModes, ref Span<int> memory, Runtime runtime)
        {
            var parameter = GetParameters(index, parameterModes, memory);

            runtime.Output.Push(parameter[0]);

            index += 2;
        }
    }
}
