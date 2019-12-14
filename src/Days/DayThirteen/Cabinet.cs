using Helpers;
using Helpers.Points;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DayThirteen
{
    public class Cabinet
    {
        private readonly IntcodeComputer _computer;

        private long _score = 0;
        private readonly Dictionary<Point, TileType> _tiles = new Dictionary<Point, TileType>();

        public ImmutableDictionary<Point, TileType> Tiles => _tiles.ToImmutableDictionary();

        public Cabinet(ImmutableArray<long> memory)
        {
            _computer = new IntcodeComputer(memory);
        }

        public long Run()
        {
            var result = IntcodeResult.NO_RESULT;
            while (result != IntcodeResult.HALT_TERMINATE)
            {
                result = _computer.Run();
                ParseOutput(_computer.Output);
                if (result == IntcodeResult.HALT_FORINPUT)
                {
                    var ball = _tiles.First(x => x.Value == TileType.Ball).Key;
                    var paddle = _tiles.First(x => x.Value == TileType.HorizontalPaddle).Key;

                    var move = paddle switch
                    {
                        _ when paddle.X == ball.X => 0,
                        _ when paddle.X > ball.X => -1,
                        _ when paddle.X < ball.X => 1,
                        _ => throw new InvalidOperationException("Da Faq?"),
                    };
                    _computer.Input.Enqueue(move);
                }
            }

            return _score;
        }

        private void ParseOutput(Queue<long> output)
        {
            while (output.Count > 0)
            {
                var x = output.Dequeue();
                var y = output.Dequeue();
                var val = output.Dequeue();

                if (x == -1)
                {
                    _score = val;
                }
                else
                {
                    _tiles[new Point((int)x, (int)y)] = (TileType)val;
                }
            }
        }
    }
}
