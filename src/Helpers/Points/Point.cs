﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Helpers.Points
{
    public class Point : IEquatable<Point>
    {
        public static Point Origin { get; } = new Point(0, 0);

        private readonly ImmutableArray<int> _coords;

        public Point(int x, int y)
        {
            _coords = ImmutableArray.Create(x, y);
        }

        public int X => _coords[0];

        public int Y => _coords[1];

        public override string ToString() => $"{X}, {Y}";

        public override bool Equals(object? obj)
        {
            return Equals(obj as Point);
        }

        public bool Equals(Point? other)
        {
            return !(other is null) &&
                   X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Point left, Point right)
        {
            return EqualityComparer<Point>.Default.Equals(left, right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        public static Point operator +(Point left, Point right)
            => new Point(left.X + right.X, left.Y + right.Y);

        public static Point operator +(Point point, Direction direction) => direction.DirectionType switch
        {
            DirectionType.North => point + (0, 1),
            DirectionType.East => point + (1, 0),
            DirectionType.South => point + (0, -1),
            DirectionType.West => point + (-1, 0),
            _ => point
        };

        public static implicit operator Point((int X, int Y) i) => new Point(i.X, i.Y);

        public void Deconstruct(out int x, out int y)
        {
            x = _coords[0];
            y = _coords[1];
        }
    }
}
