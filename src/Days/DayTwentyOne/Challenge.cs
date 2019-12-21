using Helpers;
using Helpers.Computer;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayTwentyOne
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base (day: 21)
        {
        }

        public override string Name => "Springdroid Adventure";

        public void PartOne(string input, TextWriter @out)
        {
            var program = IntcodeParser.Parse(input);
            var droid = new Springdroid(program);

            var instr = ImmutableArray.Create(new[]
            {
                new Instruction(Operation.NOT, Register.T, WriteRegister.T), // TRUE
                new Instruction(Operation.AND, Register.A, WriteRegister.T), // A
                new Instruction(Operation.AND, Register.B, WriteRegister.T), // A AND B
                new Instruction(Operation.AND, Register.C, WriteRegister.T), // A AND B AND C
                new Instruction(Operation.NOT, Register.T, WriteRegister.J), // !A OR !B OR !C
                new Instruction(Operation.AND, Register.D, WriteRegister.J), // (!A OR !B OR !C) AND D
                new Instruction("WALK")
            });

            var hullDamage = droid.Run(instr, @out);

            @out.WriteLine($"Hull damange reported: {hullDamage}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var program = IntcodeParser.Parse(input);
            var droid = new Springdroid(program);

            var instr = ImmutableArray.Create(new[]
            {
                new Instruction(Operation.NOT, Register.T, WriteRegister.T), // TRUE
                new Instruction(Operation.AND, Register.A, WriteRegister.T), // A
                new Instruction(Operation.AND, Register.B, WriteRegister.T), // A AND B
                new Instruction(Operation.AND, Register.C, WriteRegister.T), // A AND B AND C
                new Instruction(Operation.NOT, Register.T, WriteRegister.T), // !A OR !B OR !C
                new Instruction(Operation.NOT, Register.J, WriteRegister.J), // TRUE
                new Instruction(Operation.AND, Register.E, WriteRegister.J), // E
                new Instruction(Operation.OR , Register.H, WriteRegister.J), // E OR H
                new Instruction(Operation.AND, Register.T, WriteRegister.J), // (!A OR !B OR !C) AND (E OR H)
                new Instruction(Operation.AND, Register.D, WriteRegister.J), // (!A OR !B OR !C) AND (E OR H) AND D
                new Instruction("RUN")
            });

            var hullDamage = droid.Run(instr, @out);

            @out.WriteLine($"Hull damange reported: {hullDamage}");
        }
    }
}
