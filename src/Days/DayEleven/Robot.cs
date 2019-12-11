using Helpers;
using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DayEleven
{
    public class Robot
    {
        public Point Position { get; private set; }

        public Facing Facing { get; private set; } = Facing.North;

        public Robot(Point startPosition)
        {
            Position = startPosition;
        }

        public void TurnRight()
        {
            Facing = Facing switch
            {
                Facing.North => Facing.East,
                Facing.East => Facing.South,
                Facing.South => Facing.West,
                Facing.West => Facing.North,
                _ => throw new InvalidOperationException("What the facing!"),
            };
            MoveForward();
        }

        public void TurnLeft()
        {
            Facing = Facing switch
            {
                Facing.North => Facing.West,
                Facing.East => Facing.North,
                Facing.South => Facing.East,
                Facing.West => Facing.South,
                _ => throw new InvalidOperationException("What the facing!"),
            };
            MoveForward();
        }

        public void MoveForward()
        {
            Position = Facing switch
            {
                Facing.North => Position + new Point(0, -1),
                Facing.East => Position + new Point(1, 0),
                Facing.South => Position + new Point(0, 1),
                Facing.West => Position + new Point(-1, 0),
                _ => throw new InvalidOperationException("What the facing!"),
            };
        }

        public ImmutableDictionary<Point, int> RunProgram(ImmutableArray<long> memory, Dictionary<Point, int>? startingPainted = null)
        {
            var painted = ImmutableDictionary.CreateBuilder<Point, int>();
            if (startingPainted != null)
            {
                painted.AddRange(startingPainted);
            }

            var computer = new IntcodeComputer(memory);

            while (true)
            {
                var colour = painted.TryGetValue(Position, out var currentColour)
                    ? currentColour
                    : 0;

                computer.Input.Enqueue(colour);
                var result = computer.Run();

                if (result == IncodeResult.HALT_TERMINATE)
                {
                    break;
                }

                var outColour = computer.Output.Dequeue();
                painted[Position] = (int)outColour;

                var move = computer.Output.Dequeue();
                if (move == 0)
                {
                    TurnLeft();
                }
                else if (move == 1)
                {
                    TurnRight();
                }
            }

            return painted.ToImmutable();
        }
    }
}
