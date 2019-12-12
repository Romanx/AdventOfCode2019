using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Helpers.Points
{
    public class Point3d : IEquatable<Point3d>
    {
        private readonly ImmutableArray<int> _coords;

        public static Point3d Origin { get; } = new Point3d(0, 0, 0);

        public Point3d(int x, int y, int z)
        {
            _coords = ImmutableArray.Create(x, y, z);
        }

        public int X => _coords[0];

        public int Y => _coords[1];

        public int Z => _coords[2];

        public int this[int index] => _coords[index];

        public override bool Equals(object? obj)
        {
            return Equals(obj as Point3d);
        }

        public bool Equals(Point3d? other)
        {
            return other is object &&
                   X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public override string ToString()
            => $"{X}, {Y}, {Z}";

        public static bool operator ==(Point3d left, Point3d right)
        {
            return EqualityComparer<Point3d>.Default.Equals(left, right);
        }

        public static bool operator !=(Point3d left, Point3d right)
        {
            return !(left == right);
        }

        public static Point3d operator +(Point3d left, Point3d right)
            => new Point3d(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }
    }
}
