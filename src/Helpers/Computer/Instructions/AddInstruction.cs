using System;

namespace Helpers.Instructions
{
    internal class AddInstruction : Instruction
    {
        public override int OpCode => 1;

        public override ParameterType[] Parameters { get; } = new[]
        {
            ParameterType.Read,
            ParameterType.Read,
            ParameterType.Write
        };

        public override void RunInstruction(ref int index, in ReadOnlySpan<int> parameterModes, ref Span<int> memory, Runtime runtime)
        {
            var parameters = GetParameters(index, parameterModes, memory);

            memory[parameters[2]] = parameters[0] + parameters[1];

            index += 4;
        }
    }
}

