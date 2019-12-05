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
            => Compute(memory, new Runtime(new Stack<int>()));

        public static int Compute(ImmutableArray<int> memory, Runtime runtime)
        {
            var scratch = new int[memory.Length];
            memory.CopyTo(scratch);

            var memorySpan = scratch.AsSpan();
            int index = 0;

            while (memorySpan[index] != HaltCode)
            {
                var op = memorySpan[index];
                var (opCode, parameterModes) = ParseOperation(op);

                if (_validInstructions.TryGetValue(opCode, out var instruction))
                {
                    instruction.RunInstruction(ref index, parameterModes, ref memorySpan, runtime);
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

        private static (int opCode, int[] parameterModes) ParseOperation(int op)
        {
            var operation = op.ToString().ToArray();
            if (operation.Length == 1)
            {
                return (op, Array.Empty<int>());
            }

            var opCode = int.Parse(operation[^2..]);

            if (operation.Length > 2)
            {
                var parameters = operation[..^2].Select(c => c - '0').Reverse().ToArray();

                return (opCode, parameters);
            }

            return (opCode, Array.Empty<int>());
        }
    }

    public class Runtime
    {
        public Runtime(Stack<int> input)
        {
            Input = input;
        }

        public Stack<int> Input { get; }

        public Stack<int> Output { get; } = new Stack<int>();
    }
}
