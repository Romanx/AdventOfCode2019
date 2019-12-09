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

        private long[] _memory;
        private int _index = 0;
        private int _relativeBase = 0;

        public Queue<long> Input { get; } = new Queue<long>();

        public Queue<long> Output { get; } = new Queue<long>();

        public IntcodeComputer(ImmutableArray<long> memory)
        {
            var scratch = new long[memory.Length];
            scratch.Populate(0);
            memory.CopyTo(scratch);

            _memory = scratch;
        }

        public IncodeResult Run()
        {
            while (_memory[_index] != HaltCode)
            {
                var op = _memory[_index];
                var (opCode, parameterModes) = ParseOperation(op);

                if (_validInstructions.TryGetValue(opCode, out var instruction))
                {
                    if (opCode == OpCodes.Read && Input.Count == 0)
                    {
                        return IncodeResult.HALT_FORINPUT;
                    }

                    var @params = GetParameters(instruction.Parameters, parameterModes);
                    instruction.RunInstruction(@params, this);
                }
                else
                {
                    throw new InvalidOperationException($"Invalid op code: {opCode}");
                }
            }

            if (Output.Count == 0)
            {
                Output.Enqueue(_memory[0]);
            }

            return IncodeResult.HALT_TERMINATE;
        }

        public void SetIndex(int index) => _index = index;

        public long ReadMemory(int position) => _memory[position];

        public void WriteToMemory(int position, long value)
        {
            if (position > _memory.Length)
            {
                IncreaseMemory();
            }

            _memory[position] = value;
        }

        public void AdjustIndexBy(int value) => _index += value;

        public void AdjustRelativeBaseBy(int value) => _relativeBase += value;

        private static (OpCodes opCode, int[] parameterModes) ParseOperation(long op)
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

        private ReadOnlySpan<long> GetParameters(
            ParameterType[] parameterTypes,
            in ReadOnlySpan<int> parameterModes)
        {
            var paramStart = _index + 1;
            var readParameters = _memory[paramStart..(paramStart + parameterTypes.Length)];

            var modes = new int[parameterTypes.Length];
            modes.Populate(0);
            parameterModes.CopyTo(modes);

            var results = new long[parameterTypes.Length];

            for (var i = 0; i < readParameters.Length; i++)
            {
                var type = parameterTypes[i];
                var param = readParameters[i];
                var mode = (ParameterMode)modes[i];

                switch (mode)
                {
                    case ParameterMode.PositionMode when type == ParameterType.Write:
                        results[i] = param;
                        break;
                    case ParameterMode.PositionMode:
                        if (param > _memory.Length)
                            IncreaseMemory();

                        results[i] = _memory[param];
                        break;
                    case ParameterMode.ImmediateMode:
                        if (type == ParameterType.Write)
                            throw new InvalidOperationException("Cannot write with immedate mode");

                        results[i] = param;
                        break;
                    case ParameterMode.RelativeMode when type == ParameterType.Write:
                        results[i] = _relativeBase + param;
                        break;

                    case ParameterMode.RelativeMode:
                        if (_relativeBase + param > _memory.Length)
                            IncreaseMemory();
                        results[i] = _memory[_relativeBase + param];
                        break;
                    default:
                        throw new InvalidOperationException("Invalid Memory Mode!");
                }
            }

            return results;
        }

        private void IncreaseMemory()
        {
            var newMemory = new long[(int)Math.Pow(_memory.Length, 2)];
            newMemory.Populate(0);
            _memory.CopyTo(newMemory.AsSpan());
            _memory = newMemory;
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

    internal enum ParameterMode
    {
        PositionMode = 0,
        ImmediateMode = 1,
        RelativeMode = 2
    }
}
