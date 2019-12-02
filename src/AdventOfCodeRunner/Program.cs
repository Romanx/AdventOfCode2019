using DayOne;
using Helpers;
using System;
using Zio;
using Zio.FileSystems;

namespace AdventOfCodeRunner
{
    public class Program
    {
        private static readonly ChallengeBase[] Challenges = new ChallengeBase[]
        {
            new DayOneChallenge(),
            new DayTwo.Challenge()
        };

        public static void Main(string[] args)
        {
            var fs = new PhysicalFileSystem();
            var day = int.Parse(args[0]);
            var challenge = Challenges[day - 1];

            UPath basePath = fs.ConvertPathFromInternal(AppDomain.CurrentDomain.BaseDirectory);
            var inputPath = basePath / "../../../../../input";

            var file = fs.GetFileEntry(inputPath / $"day-{day}.txt");

            RunChallenge(Console.Out, file, challenge);
        }

        private static void RunChallenge(System.IO.TextWriter @out, FileEntry file, ChallengeBase challenge)
        {
            switch (challenge)
            {
                case INeedLines challengeNeedsLines:
                    RunWithLines(@out, file.ReadAllLines(), challengeNeedsLines);
                    break;
                case INeedAllInput challengeNeedsAllInput:
                    RunWithAllInput(@out, file.ReadAllText(), challengeNeedsAllInput);
                    break;
                default:
                    throw new InvalidOperationException("Must implement marker interface");
            }
        }

        private static void RunWithLines(System.IO.TextWriter @out, string[] lines, INeedLines challenge)
        {
            @out.WriteHeader($"{challenge.Name} - Part 1");
            challenge.PartOne(lines, @out);

            @out.WriteLine();

            @out.WriteHeader($"{challenge.Name} - Part 2");
            challenge.PartTwo(lines, @out);
        }

        private static void RunWithAllInput(System.IO.TextWriter @out, string content, INeedAllInput challenge)
        {
            @out.WriteHeader($"{challenge.Name} - Part 1");
            challenge.PartOne(content, @out);

            @out.WriteLine();

            @out.WriteHeader($"{challenge.Name} - Part 2");
            challenge.PartTwo(content, @out);
        }
    }
}
