using Helpers;
using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DayTwelve
{
    public static class Simulator
    {
        public static ImmutableList<(Point3d Position, Point3d Velocity)> Simulate(int steps, ImmutableList<Point3d> startingState)
        {
            var state = startingState
                .Select(pos => (Position: pos, Velocity: Point3d.Origin))
                .ToImmutableList();

            for (var i = 0; i < steps; i++)
            {
                state = RunStep(state);
            }

            return state;
        }

        private static ImmutableList<(Point3d Position, Point3d Velocity)> RunStep(ImmutableList<(Point3d Position, Point3d Velocity)> state)
        {
            var result = ImmutableList.CreateBuilder<(Point3d Position, Point3d Velocity)>();

            foreach (var point in state)
            {
                var updated = UpdateAndApplyVelocity(point, state.Select(s => s.Position).Where(s => s != point.Position));
                result.Add(updated);
            }

            return result.ToImmutable();
        }

        public static long SimulateUntilLoop(ImmutableList<Point3d> startingState)
        {
            var rootState = startingState
                   .Select(pos => (Position: pos, Velocity: Point3d.Origin))
                   .ToImmutableList();

            var state = rootState;
            var cycle = new long[3];
            cycle.Populate(0);

            var shouldContinue = false;
            long step = 0;
            do
            {
                state = RunStep(state);
                step++;
                shouldContinue = false;
                for (var i = 0; i < 3; i++)
                {
                    if (cycle[i] != 0) continue;
                    shouldContinue = true;

                    var isMatch = state.Select((x, idx) => MatchPositionAtIndexAndZeroVelocity(i, startingState[idx], x.Position, x.Velocity)).All(x => x);

                    if (isMatch)
                    {
                        cycle[i] = step;
                    }
                }

            } while (shouldContinue);

            return cycle.Aggregate(1L, LCM);

            static bool MatchPositionAtIndexAndZeroVelocity(int index, Point3d startPosition, Point3d position, Point3d velocity)
            {
                return startPosition[index] == position[index] && velocity[index] == 0;
            }
        }

        private static (Point3d Position, Point3d Velocity) UpdateAndApplyVelocity((Point3d Position, Point3d Velocity) moon, IEnumerable<Point3d> otherMoons)
        {
            var (x, y, z) = moon.Position;
            var newVelocity = moon.Velocity;
            foreach (var other in otherMoons)
            {
                newVelocity += new Point3d(
                    CalculateChange(x, other.X),
                    CalculateChange(y, other.Y),
                    CalculateChange(z, other.Z));
            }

            return (
                moon.Position + newVelocity,
                newVelocity
            );

            static int CalculateChange(int axisA, int axisB)
            {
                if (axisA == axisB)
                {
                    return 0;
                }

                if (axisB > axisA)
                {
                    return 1;
                }

                return -1;
            }
        }

        /// <summary>
        /// Find the Greatest common divisor between two numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        /// <summary>
        /// Returns the lowest common multiple of two numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static long LCM(long a, long b)
        {
            return a * b / GCD(a, b);
        }
    }
}
