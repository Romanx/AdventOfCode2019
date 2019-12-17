using System;

namespace Helpers
{
    public static class NumberHelpers
    {
        public static int DigitAtPosition(this int num, uint pos) => (num / (int)Math.Pow(10, pos - 1)) % 10;
    }
}
