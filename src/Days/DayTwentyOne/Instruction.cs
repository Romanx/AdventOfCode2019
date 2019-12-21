using System.Collections.Generic;
using System.Linq;

namespace DayTwentyOne
{
    public struct Instruction
    {
        public Instruction(Operation op, Register a, WriteRegister b)
        {
            Command = $"{op} {a} {b}";
        }

        public Instruction(string command)
        {
            Command = command;
        }

        public string Command { get; }
    }
}
