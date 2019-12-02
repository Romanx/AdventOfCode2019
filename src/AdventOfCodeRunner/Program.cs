using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Zio;
using Zio.FileSystems;

namespace AdventOfCodeRunner
{
    public class Program
    {
        private static readonly Dictionary<int, ChallengeBase> Challenges = LoadChallenges();

        public static void Main(string[] args)
        {
            var day = args.Length == 1
                ? int.Parse(args[0])
                : GetDay(Console.Out);

            var fs = new PhysicalFileSystem();
            UPath basePath = fs.ConvertPathFromInternal(AppDomain.CurrentDomain.BaseDirectory);
            var inputPath = basePath / "../../../../../input";

            var challenge = Challenges[day];
            var file = fs.GetFileEntry(inputPath / $"day-{day}.txt");

            Console.Clear();
            RunChallenge(Console.Out, file, challenge);
        }

        private static int GetDay(System.IO.TextWriter @out)
        {
            Console.Clear();
            @out.WriteHeader("Advent of Code!");
            foreach (var item in Challenges.Values.OrderBy(x => x.Day))
            {
                @out.WriteLine($"{item.Day}) {item.Name}");
            }
            @out.WriteLine();
            @out.Write("Please select the day you want to run >  ");
            var day = Console.ReadLine();
            return int.Parse(day);
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
            @out.WriteHeader($"Day {challenge.Day}: {challenge.Name} - Part 1");
            challenge.PartOne(lines, @out);

            @out.WriteLine();

            @out.WriteHeader($"Day {challenge.Day}: {challenge.Name} - Part 2");
            challenge.PartTwo(lines, @out);
        }

        private static void RunWithAllInput(System.IO.TextWriter @out, string content, INeedAllInput challenge)
        {
            @out.WriteHeader($"Day {challenge.Day}: {challenge.Name} - Part 1");
            challenge.PartOne(content, @out);

            @out.WriteLine();

            @out.WriteHeader($"Day {challenge.Day}: {challenge.Name} - Part 2");
            challenge.PartTwo(content, @out);
        }

        private static Dictionary<int, ChallengeBase> LoadChallenges()
        {
            var context = new CollectibleAssemblyLoadContext();
            var fs = new PhysicalFileSystem();
            UPath basePath = fs.ConvertPathFromInternal(AppDomain.CurrentDomain.BaseDirectory);

            var challenges = fs.EnumerateFileEntries(basePath, "Day*.dll")
                .Select(entry => LoadChallenge(entry, context))
                .ToDictionary(k => k.Day, v => v);

            return challenges;

            static ChallengeBase LoadChallenge(FileEntry entry, CollectibleAssemblyLoadContext context)
            {
                using var fs = entry.Open(System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var assembly = context.LoadFromStream(fs);

                return assembly
                    .GetTypes()
                    .Where(t => !t.IsAbstract && typeof(ChallengeBase).IsAssignableFrom(t))
                    .Select(t => (ChallengeBase)Activator.CreateInstance(t)!)
                    .First();
            }
        }
    }
}
