using Helpers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DaySeven
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 7)
        {
        }

        public override string Name => "Amplification Circuit";

        public void PartOne(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(int.Parse)
                .ToImmutableArray();

            var phaseCombinations = GeneratePhaseSettings(0);

            var bag = new ConcurrentBag<(int[] PhaseCombination, int ThrusterOutput)>();

            Parallel.ForEach(phaseCombinations, phaseCombination =>
            {
                var res = RunThrusterProgram(memory, phaseCombination);

                bag.Add((phaseCombination, res));
            });

            var (phaseCombination, thrusterOutput) = bag
                .OrderByDescending(x => x.ThrusterOutput)
                .First();

            @out.WriteLine($"Max thruster signal {thrusterOutput} (from phase setting sequence {string.Join(",", phaseCombination)})");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(int.Parse)
                   .ToImmutableArray();

            var phaseCombinations = GeneratePhaseSettings(5)
                .ToArray();

            var bag = new ConcurrentBag<(int[] PhaseCombination, int ThrusterOutput)>();

            Parallel.ForEach(phaseCombinations, phaseCombination =>
            {
                var res = RunThrusterProgramInFeedback(memory, phaseCombination);

                bag.Add((phaseCombination, res));
            });

            var (phaseCombination, thrusterOutput) = bag
                .OrderByDescending(x => x.ThrusterOutput)
                .First();

            @out.WriteLine($"Max thruster signal {thrusterOutput} (from phase setting sequence {string.Join(",", phaseCombination)})");
        }

        public int RunThrusterProgram(ImmutableArray<int> memory, int[] phaseCombination)
        {
            var signal = 0;
            foreach (var phase in phaseCombination)
            {
                var computer = new IntcodeComputer(memory);
                computer.Input.Enqueue(phase);
                computer.Input.Enqueue(signal);

                computer.Run();

                signal = computer.Output.Dequeue();
            }

            return signal;
        }

        public int RunThrusterProgramInFeedback(ImmutableArray<int> memory, int[] phaseCombination)
        {
            var computers = new List<IntcodeComputer>();
            var thrusters = new Queue<IntcodeComputer>(phaseCombination.Length);
            for (var i = 0; i < phaseCombination.Length; i++)
            {
                var computer = new IntcodeComputer(memory);
                computer.Input.Enqueue(phaseCombination[i]);

                computers.Add(computer);
                thrusters.Enqueue(computer);
            }

            var signal = 0;
            while (thrusters.Count > 0)
            {
                var computer = thrusters.Dequeue();
                computer.Input.Enqueue(signal);
                var result = computer.Run();

                signal = computer.Output.Dequeue();
                if (result == IncodeResult.HALT_FORINPUT)
                {
                    thrusters.Enqueue(computer);
                }
            }

            return signal;
        }

        public IEnumerable<int[]> GeneratePhaseSettings(int start)
        {
            foreach (var i in Enumerable.Range(start, 5))
            {
                foreach (var j in Enumerable.Range(start, 5))
                {
                    foreach (var k in Enumerable.Range(start, 5))
                    {
                        foreach (var l in Enumerable.Range(start, 5))
                        {
                            foreach (var m in Enumerable.Range(start, 5))
                            {
                                var rs = new[] { i, j, k, l, m };
                                if (rs.Distinct().Count() == 5)
                                    yield return rs;
                            }
                        }
                    }
                }
            }
        }
    }
}
