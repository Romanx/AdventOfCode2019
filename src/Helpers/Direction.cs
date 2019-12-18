using System;

namespace Helpers
{
    public enum DirectionType
    {
        North,
        South,
        East,
        West
    }

    public class Direction
    {
        public static Direction North { get; } = new Direction(DirectionType.North);
        public static Direction East { get; } = new Direction(DirectionType.East);
        public static Direction South { get; } = new Direction(DirectionType.South);
        public static Direction West { get; } = new Direction(DirectionType.West);

        private static readonly Direction[] values = new[]
        {
            North,
            East,
            South,
            West
        };

        public DirectionType DirectionType { get; }

        private Direction(DirectionType direction)
        {
            DirectionType = direction;
        }

        public Direction Right() => values[(Array.IndexOf(values, this) + 1) % 4];

        public Direction Left() => values[(Array.IndexOf(values, this) + 3) % 4];
    }
}
