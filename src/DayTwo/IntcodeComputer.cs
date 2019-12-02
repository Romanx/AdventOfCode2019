using System;

namespace DayTwo
{
    internal class IntcodeComputer
    {
        public static int Compute(int[] memory)
        {
            for (var i = 0; ; i += 4)
            {
                var op = memory[i];

                if (op == 99)
                    break;

                RunOperation(op, i, memory);
            }

            return memory[0];

            static void RunOperation(int op, int index, int[] memory)
            {
                var (aValue, bValue, outAddress) = ReadOpData(memory, memory[index..(index + 4)]);

                memory[outAddress] = op switch
                {
                    1 => aValue + bValue,
                    2 => aValue * bValue,
                    _ => throw new InvalidOperationException("Invalid op code"),
                };
            }
        }

        private static (int AValue, int BValue, int OutAddress) ReadOpData(ReadOnlySpan<int> operations, ReadOnlySpan<int> operationSpan)
        {
            var aAddress = operationSpan[1];
            var bAddress = operationSpan[2];
            var outAddress = operationSpan[3];

            var aValue = operations[aAddress];
            var bValue = operations[bAddress];

            return (aValue, bValue, outAddress);
        }
    }
}
