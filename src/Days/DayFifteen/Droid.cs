using Helpers;
using Helpers.Points;
using System;
using System.Collections.Immutable;
using System.Linq;
using static DayFifteen.DirectionHelpers;

namespace DayFifteen
{
    public class Droid
    {
        private readonly ImmutableDictionary<Point, CellType>.Builder _map;
        private readonly IntcodeComputer _computer;
        private Point _location;

        public Droid(ImmutableArray<long> program)
        {
            _computer = new IntcodeComputer(program);
            _map = ImmutableDictionary.CreateBuilder<Point, CellType>();
            _location = Point.Origin;
        }

        public ImmutableDictionary<Point, CellType> Run()
        {
            Discover(_location);
            return _map.ToImmutable();
        }

        void Discover(Point point)
        {
            CheckDirection(Direction.North);
            CheckDirection(Direction.East);
            CheckDirection(Direction.South);
            CheckDirection(Direction.West);

            void CheckDirection(Direction direction)
            {
                var newLocation = LocationAtDirection(point, direction);
                if (_map.ContainsKey(newLocation)) return;
                MoveDirection(direction);
            }
        }

        private void MoveDirection(Direction direction)
        {
            var status = JustMove(direction);
            var nextLocation = LocationAtDirection(_location, direction);

            _map.Add(nextLocation, status);

            if (status != CellType.Wall)
            {
                _location = nextLocation;
                Discover(_location);
                UndoDirection(direction);
            }
        }

        private CellType JustMove(Direction direction)
        {
            _computer.Input.Enqueue((int)direction);
            _computer.Run();
            return (CellType)_computer.Output.Dequeue();
        }

        private void UndoDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    JustMove(Direction.South);
                    _location = LocationAtDirection(_location, Direction.South);
                    break;
                case Direction.East:
                    JustMove(Direction.West);
                    _location = LocationAtDirection(_location, Direction.West);
                    break;
                case Direction.South:
                    JustMove(Direction.North);
                    _location = LocationAtDirection(_location, Direction.North);
                    break;
                case Direction.West:
                    JustMove(Direction.East);
                    _location = LocationAtDirection(_location, Direction.East);
                    break;
            }
        }
    }
}
