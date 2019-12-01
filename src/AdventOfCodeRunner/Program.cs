using DayOne;
using Helpers;
using System;
using System.Threading.Tasks;
using Zio;
using Zio.FileSystems;

namespace AdventOfCodeRunner
{
    public class Program
    {
        private static readonly ChallengeBase[] Challenges = new[]
        {
            new DayOneChallenge()
        };

        public static async Task Main(string[] args)
        {
            var fs = new PhysicalFileSystem();
            var day = int.Parse(args[0]);
            var challenge = Challenges[day - 1];

            UPath basePath = fs.ConvertPathFromInternal(AppDomain.CurrentDomain.BaseDirectory);
            var inputPath = basePath / "../../../../../input";

            var file = fs.ReadAllLines(inputPath / $"day-{day}.txt");

            await RunChallenge(Console.Out, file, challenge).ConfigureAwait(false);
        }

        private static async Task RunChallenge(System.IO.TextWriter @out, string[] lines, ChallengeBase challenge)
        {
            await @out.WriteHeader($"{challenge.Name} - Part 1");
            await challenge.PartOne(lines, @out).ConfigureAwait(false);

            await @out.WriteLineAsync();

            await @out.WriteHeader($"{challenge.Name} - Part 2");
            await challenge.PartTwo(lines, @out).ConfigureAwait(false);
        }
    }
}
