using System;

namespace Helpers.Instructions
{
    internal class AddInstruction : Instruction
    {
        public override int OpCode => 1;

        public override void RunInstruction(ref int index, ref Span<int> memory)
        {
            var (aValue, bValue, outAddress) = GetParameters(ref index, ref memory);

            memory[outAddress] = aValue + bValue;
            index += 4;
        }

        private (int AValue, int BValue, int OutAddress) GetParameters(ref int index, ref Span<int> memory)
        {
            var parameters = memory[index..(index + 4)];

            var aAddress = parameters[1];
            var bAddress = parameters[2];
            var outAddress = parameters[3];

            var aValue = memory[aAddress];
            var bValue = memory[bAddress];

            return (aValue, bValue, outAddress);
        }
    }
}

