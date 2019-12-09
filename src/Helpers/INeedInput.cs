using System.IO;

namespace Helpers
{
    public interface INeedInput<T>
    {
        void PartOne(T input, TextWriter @out);

        void PartTwo(T input, TextWriter @out);

    }
}
