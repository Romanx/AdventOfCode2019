using System;

namespace Helpers.Instructions
{
    internal abstract class Instruction
    {
        public abstract int OpCode { get; }

        public abstract void RunInstruction(ref int index, ref Span<int> memory);
    }
}
