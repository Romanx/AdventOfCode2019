using System;

namespace Helpers.Instructions
{
    internal abstract class Instruction
    {
        public abstract OpCodes OpCode { get; }

        public abstract ParameterType[] Parameters { get; }

        public abstract void RunInstruction(ref int index, in ReadOnlySpan<int> parameters, IntcodeComputer runtime, ref int[] memory);

        public ReadOnlySpan<int> GetParameters(
            in int index,
            in ReadOnlySpan<int> parameterModes,
            in ReadOnlySpan<int> memory)
        {
            var paramStart = index + 1;
            var readParameters = memory[paramStart..(paramStart + Parameters.Length)];
            
            var modes = new int[Parameters.Length];
            modes.Populate(0);
            parameterModes.CopyTo(modes);

            var results = new int[Parameters.Length];

            for (var i = 0; i < readParameters.Length; i++)
            {
                var type = Parameters[i];
                var param = readParameters[i];
                var mode = modes[i];

                if (type == ParameterType.Write)
                {
                    results[i] = param;
                    continue;
                }

                results[i] = mode switch
                {
                    0 => memory[param],
                    1 => param,
                    _ => throw new InvalidOperationException("Invalid Memory Mode!"),
                };
            }

            return results;
        }
    }
}
