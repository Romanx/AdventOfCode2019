namespace DayTwentyThree
{
    public struct Packet
    {
        public Packet(long x, long y)
        {
            X = x;
            Y = y;
        }

        public long X { get; }
        public long Y { get; }
    }
}
