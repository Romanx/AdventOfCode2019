using System.IO;
using System.Threading.Tasks;

namespace Helpers
{
    public static class TextWriterExtensions
    {
        public static void WriteHeader(this TextWriter writer, string header)
        {
            writer.WriteLine(new string('*', header.Length + 6));
            writer.WriteLine($"   {header}   ");
            writer.WriteLine(new string('*', header.Length + 6));
        }
    }
}
