using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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
            var root = basePath / "../../../../../";
            var inputPath = root / "input";
            var outputPath = root / "output";

            var challenge = Challenges[day];
            var file = fs.GetFileEntry(inputPath / $"day-{day}.txt");
            challenge.OutDirectory = CreateDayOuputDirectory(day, outputPath, fs);

            Console.Clear();
            RunChallenge(Console.Out, file, challenge);
        }

        private static int GetDay(TextWriter @out)
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

        private static void RunChallenge(TextWriter @out, FileEntry file, ChallengeBase challenge)
        {
            switch (challenge)
            {
                case INeedLines needLines:
                    Run(@out, file.ReadAllLines(), needLines, challenge);
                    break;
                case INeedAllInput needAll:
                    Run(@out, file.ReadAllText(), needAll, challenge);
                    break;
                default:
                    throw new InvalidOperationException("Must implement marker interface");
            }
        }

        private static void Run<T>(TextWriter @out, T input, INeedInput<T> runner, ChallengeBase challenge)
        {
            RunWithInput(@out, input, WriteHeader(1, challenge), (writer, input) => runner.PartOne(input, writer));
            RunWithInput(@out, input, WriteHeader(2, challenge), (writer, input) => runner.PartTwo(input, writer));
        }

        private static void RunWithInput<T>(
            TextWriter @out,
            T input,
            Action<TextWriter> headerWriter,
            Action<TextWriter, T> runStep) 
        {
            try
            {
                headerWriter(@out);
                runStep(@out, input);
            }
            catch (NotImplementedException)
            {
                @out.WriteLine("Not Yet Implemented".ToUpper());
            }
        }

        private static Action<TextWriter> WriteHeader(int part, ChallengeBase challenge) 
            => (TextWriter writer) => writer.WriteHeader($"Day {challenge.Day}: {challenge.Name} - Part {part}");

        private static DirectoryEntry CreateDayOuputDirectory(int day, UPath outputPath, FileSystem fs)
        {
            var dir = outputPath / $"day-{day}";

            if (fs.DirectoryExists(dir))
            {
                fs.DeleteDirectory(dir, true);
            }
            fs.CreateDirectory(dir);

            return fs.GetDirectoryEntry(dir);
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
                using var fs = entry.Open(FileMode.Open, FileAccess.Read);
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
