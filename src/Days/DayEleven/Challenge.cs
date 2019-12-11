using Helpers;
using Helpers.Points;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayEleven
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base (day: 11)
        {
        }

        public override string Name => "Space Police";

        public void PartOne(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(long.Parse).ToImmutableArray();
            var robot = new Robot(new Point(0, 0));

            var results = robot.RunProgram(memory);

            @out.WriteLine($"Number of points painted: {results.Count}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var memory = input.Split(',').Select(long.Parse).ToImmutableArray();
            var start = new Point(0, 0);

            var robot = new Robot(start);
            var painted = new Dictionary<Point, int>
            {
                [start] = 1
            };

            var results = robot.RunProgram(memory, painted);

            WriteImage("part2.jpg", results);
            @out.WriteLine($"Image written to output directory");
        }

        private void WriteImage(string fileName, ImmutableDictionary<Point, int> points)
        {
            var file = CreateOutputFile(fileName);

            var minX = points.Min(p => p.Key.X);
            var maxX = points.Max(p => p.Key.X);
            var minY = points.Min(p => p.Key.Y);
            var maxY = points.Max(p => p.Key.Y);

            var xOffset = Math.Abs(minX);
            var yOffset = Math.Abs(minY);

            using (var image = new Image<Rgba32>(maxX + xOffset + 1 , maxY + yOffset + 1))
            {
                foreach (var (point, colour) in points)
                {
                    var (x, y) = (point.X + xOffset, point.Y + yOffset);

                    image[x, y] = colour == 0
                        ? Rgba32.Black
                        : Rgba32.White;
                }

                image.Mutate(x => x.Resize(image.Width * 10, image.Height * 10).Pixelate(10));
                image.SaveAsJpeg(file);
            }
        }
    }
}
