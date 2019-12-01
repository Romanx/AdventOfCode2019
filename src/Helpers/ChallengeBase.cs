using System.IO;
using System.Threading.Tasks;

namespace Helpers
{
    public abstract class ChallengeBase
    {
        protected ChallengeBase(int day)
        {
            Name = $"Day {day}";
        }

        public string Name { get; }

        public abstract Task PartOne(string[] lines, TextWriter @out);

        public abstract Task PartTwo(string[] lines, TextWriter @out);
    }
}
