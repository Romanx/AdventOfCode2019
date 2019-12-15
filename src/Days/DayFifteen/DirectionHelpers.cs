using Helpers.Points;
using System;

namespace DayFifteen
{
    public static class DirectionHelpers
    {
        public static Point LocationAtDirection(Point location, Direction direction) => direction switch
        {
            Direction.North => location + new Point(0, 1),
            Direction.South => location + new Point(0, -1),
            Direction.East => location + new Point(1, 0),
            Direction.West => location + new Point(-1, 0),
            _ => throw new InvalidOperationException("Invalid Direction"),
        };
    }
}
