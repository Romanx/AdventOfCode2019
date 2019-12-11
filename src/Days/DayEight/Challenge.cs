using Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;

namespace DayEight
{
    public class Challenge : ChallengeBase, INeedAllInput
    {
        public Challenge() : base(day: 8)
        {
        }

        public override string Name => "Space Image Format";

        public void PartOne(string input, TextWriter @out)
        {
            var image = Image.ParseSpaceImageFormat(input, 25, 6);

            var layer = image.Layers
                .OrderBy(layer => layer.Flatten().Count(i => i == 0))
                .First();

            var layerArray = layer.Flatten().ToArray();
            var oneDigits = layerArray.Count(i => i == 1);
            var twoDigits = layerArray.Count(i => i == 2);

            @out.WriteLine($"One Digits {oneDigits}, Two Digits {twoDigits}: {oneDigits * twoDigits}");
        }

        public void PartTwo(string input, TextWriter @out)
        {
            var image = Image.ParseSpaceImageFormat(input, 25, 6);

            Image squashed = image.Squash();
            PrintImageToConsole(squashed);
            PrintImageToOutput(squashed);
        }

        public void PrintImageToConsole(Image image)
        {
            var pixels = image.Layers[0].Pixels;
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    switch (pixels[y, x])
                    {
                        case 0:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" ");
                            break;
                        case 1:
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(" ");
                            break;
                        case 2:
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.Write(" ");
                            break;
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public void PrintImageToOutput(Image image)
        {
            var pixels = image.Layers[0].Pixels;
            var file = CreateOutputFile("part2.jpg");

            using (var outImage = new Image<Rgba32>(image.Width, image.Height))
            {
                for (var y = 0; y < image.Height; y++)
                {
                    for (var x = 0; x < image.Width; x++)
                    {
                        outImage[x, y] = pixels[y, x] == 1
                            ? Rgba32.White
                            : Rgba32.Black;
                    }
                }

                outImage.Mutate(x => x.Resize(image.Width * 10, image.Height * 10).Pixelate(10));
                outImage.SaveAsJpeg(file);
            }
        }
    }
}