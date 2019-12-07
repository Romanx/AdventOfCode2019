using Helpers.Instructions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Helpers
{
    public class IntcodeComputer
    {
        private static readonly Dictionary<OpCodes, Instruction> _validInstructions = BuildInstructions();
        const int HaltCode = 99;

        private int[] _memory;
        private int index = 0;

        public Queue<int> Input { get; } = new Queue<int>();

        public Queue<int> Output { get; } = new Queue<int>();

        public IntcodeComputer(ImmutableArray<int> memory)
        {
            var scratch = new int[memory.Length];
            memory.CopyTo(scratch);

            _memory = scratch;
        }

        public IncodeResult Run()
        {
            while (_memory[index] != HaltCode)
            {
                var op = _memory[index];
                var (opCode, parameterModes) = ParseOperation(op);

                if (_validInstructions.TryGetValue(opCode, out var instruction))
                {
                    if (opCode == OpCodes.Read && Input.Count == 0)
                    {
                        return IncodeResult.HALT_FORINPUT;
                    }

                    var @params = instruction.GetParameters(index, parameterModes, _memory);
                    instruction.RunInstruction(ref index, @params, this, ref _memory);
                }
                else
                {
                    throw new InvalidOperationException($"Invalid op code: {op}");
                }
            }

            if (Output.Count == 0)
            {
                Output.Enqueue(_memory[0]);
            }

            return IncodeResult.HALT_TERMINATE;
        }

        private static (OpCodes opCode, int[] parameterModes) ParseOperation(int op)
        {
            var operation = op.ToString().ToArray();
            if (operation.Length == 1)
            {
                return ((OpCodes)op, Array.Empty<int>());
            }

            var opCode = int.Parse(operation[^2..]);

            var parameters = operation[..^2].Select(c => c - '0').Reverse().ToArray();

            return ((OpCodes)opCode, parameters);
        }

        private static Dictionary<OpCodes, Instruction> BuildInstructions()
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
