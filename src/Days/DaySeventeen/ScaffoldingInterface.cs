using Helpers;
using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace DaySeventeen
{
    public static class ScaffoldingInterface
    {
        public static ImmutableDictionary<Point, char> Run(ImmutableArray<long> program)
        {
            var computer = new IntcodeComputer(program);
            computer.Run();

            var result = ImmutableDictionary.CreateBuilder<Point, char>();
            var x = 0;
            var y = 0;

            while (computer.Output.TryDequeue(out var value))
            {
                switch (value)
                {
                    case '#':
                    case '.':
                    case '^':
                    case '>':
                    case '<':
                    case 'V':
                        result.Add((x, y), (char)value);
                        x++;
                        break;
                    case 10:
                        y++;
                        x = 0;
                        break;
                    default:
                        throw new InvalidOperationException("What case!?");
                }
            }

            return result.ToImmutable();
        }

        internal static long RunWithInput(ImmutableArray<long> program, ImmutableArray<string> inputProgram, ImmutableArray<string[]> subsets)
        {
            var computer = new IntcodeComputer(program);
            EnqueueCommand(computer, inputProgram);
            computer.Run();
            foreach (var subset in subsets)
            {
                EnqueueCommand(computer, subset);
                computer.Run();
            }
            computer.Input.Enqueue((int)'n');
            computer.Input.Enqueue(10);
            computer.Run();

            return computer.Output.Max();
        }

        private static void EnqueueCommand(IntcodeComputer computer, IEnumerable<string> command)
        {
            var ascii = ToAscii(string.Join(",", command));

            if (ascii.Count() > 20)
            {
                Console.WriteLine("Too Many Instructions!");
                Debugger.Break();
            }

            foreach (var c in ascii)
            {
                computer.Input.Enqueue(c);
            }
            computer.Input.Enqueue(10);

            static IEnumerable<int> ToAscii(string @in) => @in.ToCharArray().Select(c => (int)c);
        }
    }
}
