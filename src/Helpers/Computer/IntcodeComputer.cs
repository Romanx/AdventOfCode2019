using Helpers.Instructions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Helpers
{
    public static class IntcodeComputer
    {
        private static readonly Dictionary<int, Instruction> _validInstructions = BuildInstructions();

        const int HaltCode = 99;

        public static int Compute(ImmutableArray<int> memory)
        {
            var scratch = new int[memory.Length];
            memory.CopyTo(scratch);

            var memorySpan = scratch.AsSpan();
            int index = 0;

            while (memorySpan[index] != HaltCode)
            {
                var op = memorySpan[index];

                if (_validInstructions.TryGetValue(op, out var instruction))
                {
                    instruction.RunInstruction(ref index, ref memorySpan);
                }
                else
                {
                    throw new InvalidOperationException($"Invalid op code: {op}");
                }
            }

            return memorySpan[0];
        }

        private static Dictionary<int, Instruction> BuildInstructions()
        {
            return typeof(Instruction)
                .Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && typeof(Instruction).IsAssignableFrom(t))
                .Select(t => (Instruction)Activator.CreateInstance(t))
                .ToDictionary(k => k.OpCode, v => v);
        }
    }
}
