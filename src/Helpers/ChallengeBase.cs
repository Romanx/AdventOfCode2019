using System.IO;
using Zio;

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

        public DirectoryEntry? OutDirectory { get; set; }

        public Stream CreateOutputFile(string fileName)
        {
            var fs = OutDirectory!.FileSystem;
            return fs.CreateFile(OutDirectory.Path / fileName);
        }
    }
}
