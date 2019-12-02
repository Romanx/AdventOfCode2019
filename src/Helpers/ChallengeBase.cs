using System.IO;

namespace Helpers
{
    public abstract class ChallengeBase
    {
        protected ChallengeBase(int day)
        {
            Day = day;
        }

        public abstract string Name { get; }

        public int Day { get; }
    }

    public interface INeedLines
    {
        string Name { get; }

        int Day { get; }

        void PartOne(string[] lines, TextWriter @out);

        void PartTwo(string[] lines, TextWriter @out);
    }

    public interface INeedAllInput
    {
        string Name { get; }

        int Day { get; }

        void PartOne(string input, TextWriter @out);

        void PartTwo(string input, TextWriter @out);
    }
}
