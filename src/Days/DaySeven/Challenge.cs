using Helpers;
using MoreLinq;
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
            var memory = input.Split(',').Select(long.Parse)
                .ToImmutableArray();

            var phaseCombinations = GeneratePhaseSettings(0);

            var bag = new ConcurrentBag<(IList<int> PhaseCombination, int ThrusterOutput)>();

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
            var memory = input.Split(',').Select(long.Parse)
                   .ToImmutableArray();

            var phaseCombinations = GeneratePhaseSettings(5)
                .ToArray();

            var bag = new ConcurrentBag<(IList<int> PhaseCombination, int ThrusterOutput)>();

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

        public int RunThrusterProgram(ImmutableArray<long> memory, IList<int> phaseCombination)
        {
            var signal = 0;
            foreach (var phase in phaseCombination)
            {
                var computer = new IntcodeComputer(memory);
                computer.Input.Enqueue(phase);
                computer.Input.Enqueue(signal);

                computer.Run();

                signal = (int)computer.Output.Dequeue();
            }

            return signal;
        }

        public int RunThrusterProgramInFeedback(ImmutableArray<long> memory, IList<int> phaseCombination)
        {
            var computers = new List<IntcodeComputer>();
            var thrusters = new Queue<IntcodeComputer>(phaseCombination.Count);
            for (var i = 0; i < phaseCombination.Count; i++)
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

                signal = (int)computer.Output.Dequeue();
                if (result == IntcodeResult.HALT_FORINPUT)
                {
                    thrusters.Enqueue(computer);
                }
            }

            return signal;
        }

        public IEnumerable<IList<int>> GeneratePhaseSettings(int start)
            => Enumerable.Range(start, 5).Permutations();
    }
}
