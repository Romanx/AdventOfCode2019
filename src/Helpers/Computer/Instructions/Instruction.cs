using System;

namespace Helpers.Instructions
{
    internal abstract class Instruction
    {
        public abstract int OpCode { get; }

        public abstract ParameterType[] Parameters { get; }

        public abstract void RunInstruction(ref int index, in ReadOnlySpan<int> parameterModes, ref Span<int> memory, Runtime runtime);

        protected ReadOnlySpan<int> GetParameters(
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

    internal struct Parameter
    {
        public Parameter(int mode, ParameterType type)
        {
            if (type == ParameterType.Write && mode != 0)
                throw new InvalidOperationException("Write must be in mode zero");
        }
    }

    internal enum ParameterType
    {
        NotSet,
        Read,
        Write
    }
}
