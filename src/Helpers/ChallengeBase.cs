using System.IO;

namespace Helpers
{
    public abstract class ChallengeBase
    {
        protected ChallengeBase(int day)
        {
            Name = $"Day {day}";
        }

        public string Name { get; }
    }

    public interface INeedLines
    {
        string Name { get; }

        void PartOne(string[] lines, TextWriter @out);

        void PartTwo(string[] lines, TextWriter @out);
    }

    public interface INeedAllInput
    {
        string Name { get; }

        void PartOne(string input, TextWriter @out);

        void PartTwo(string input, TextWriter @out);
    }
}
