using System.IO;
using System.Threading.Tasks;

namespace Helpers
{
    public static class TextWriterExtensions
    {
        public static async Task WriteHeader(this TextWriter writer, string header)
        {
            await writer.WriteLineAsync(new string('*', header.Length + 6));
            await writer.WriteLineAsync($"   {header}   ");
            await writer.WriteLineAsync(new string('*', header.Length + 6));
        }
    }
}
