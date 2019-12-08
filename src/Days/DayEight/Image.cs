using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DayEight
{
    public class Image
    {
        public ImmutableArray<Layer> Layers { get; }
        public int Width { get; }
        public int Height { get; }

        private Image(ImmutableArray<Layer> layers, int width, int height)
        {
            Layers = layers;
            Width = width;
            Height = height;
        }

        public static Image ParseSpaceImageFormat(string input, int width, int height)
        {
            var span = input.Trim()
                .AsSpan();

            var builder = ImmutableArray.CreateBuilder<Layer>();

            while (!span.IsEmpty)
            {
                var layerPixels = new int[height, width];
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        layerPixels[y, x] = span[x + (y * width)] - '0';
                    }
                }

                builder.Add(new Layer(layerPixels));
                span = span.Slice(width * height);
            }

            return new Image(builder.ToImmutable(), width, height);
        }

        internal Image Squash()
        {
            var builder = ImmutableArray.CreateBuilder<Layer>();
            var layerPixels = new int[Height, Width];
            ZeroArray(layerPixels);

            foreach (var layer in Layers.Reverse())
            {
                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        layerPixels[y, x] = layer.Pixels[y, x] switch
                        {
                            0 => 0,
                            1 => 1,
                            2 => layerPixels[y, x],
                            _ => throw new InvalidOperationException("Invalid Pixel Combination"),
                        };
                    }
                }
            }

            builder.Add(new Layer(layerPixels));
            return new Image(builder.ToImmutable(), Width, Height);

            static void ZeroArray(int[,] array)
            {
                for (int x = 0; x < array.GetLength(0); x += 1)
                {
                    for (int y = 0; y < array.GetLength(1); y += 1)
                    {
                        array[x, y] = 2;
                    }
                }
            }
        }

        public class Layer
        {
            public Layer(int[,] pixels)
            {
                Pixels = pixels;
            }

            public int[,] Pixels { get; }

            public IEnumerable<int> Flatten()
            {
                foreach (var item in Pixels)
                    yield return item;
            }
        }
    }
}