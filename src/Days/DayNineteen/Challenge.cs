using Helpers;
using Helpers.Computer;
using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace DayNineteen
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 19)
        {
        }

        public override string Name => "Tractor Beam";

        public void PartOne(string input, TextWriter @out)
        {
            var program = IntcodeParser.Parse(input);
            var drone = new Drone(program);
            var points = new HashSet<Point>();

            const int dimensions = 50;

            for (var y = 0; y < dimensions; y++)
            {
                for (var x = 0; x < dimensions; x++)
                {
                    Point point = (x, y);
                    if (drone.TestPoint(point))
                    {
                        points.Add(point);
                    }
                }
            }

            PrintVision(points, dimensions);

            @out.WriteLine($"We have {points.Count} points where the drone is pulled");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var program = IntcodeParser.Parse(input);
            var drone = new Drone(program);

            Point point = (0, 50);

            while (true)
            {
                while (!drone.TestPoint(point))
                {
                    point += (1, 0);
                }

                var topLeft = point + (99, -99);
                if (drone.TestPoint(topLeft))
                {
                    break;
                }

                point += (0, 1);
            }


            @out.WriteLine($"Largest bounding box starts at: {point}");
            @out.WriteLine($"Result of calculation is: {(point.X * 10_000) + (point.Y - 99)}");
        }

        private void PrintVision(HashSet<Point> points, int dimension)
        {
            for (var y = 0; y < dimension; y++)
            {
                for (var x = 0; x < dimension; x++)
                {
                    Point point = (x, y);
                    if (points.Contains(point))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine("");
            }
        }
    }

    public class Drone
    {
        private readonly IntcodeComputer _computer;

        public Drone(ImmutableArray<long> program)
        {
            _computer = new IntcodeComputer(program);
        }

        public bool TestPoint(Point point)
        {
            if (point.X < 0 || point.Y < 0)
                return false;

            _computer.Reset();
            _computer.Input.Enqueue(point.X);
            _computer.Input.Enqueue(point.Y);

            var result = _computer.Run();
            if (result != IntcodeResult.HALT_TERMINATE)
            {
                throw new InvalidOperationException("WTF!");
            }

            var programResult = _computer.Output.Dequeue();

            return programResult == 1;
        }
    }
}
