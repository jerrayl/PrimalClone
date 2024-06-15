using System;
using System.Collections.Generic;
using System.Linq;
using Primal.Entities;
using Primal.Extensions;
using Primal.Models;

namespace Primal.Business.Helpers
{
    public static class GridHelper
    {
        public const int MAX_DISTANCE = 16;
        public static readonly Direction[] ORDERED_DIRECTIONS = { Direction.North, Direction.NorthWest, Direction.NorthEast, Direction.SouthWest, Direction.South, Direction.SouthEast };

        public static int? GetDistanceAlongAxis(IPosition position1, IPosition position2)
        {
            if (!IsOnSameAxis(position1, position2))
            {
                return null;
            }

            return Math.Max(Math.Abs(position1.XPosition - position2.XPosition), Math.Abs(position1.YPosition - position2.YPosition));
        }

        public static List<IPosition> GetTemplate(IPosition position, Template template, int size = 1, Direction? direction = null, Corner? corner = null)
        {
            if (direction != null && corner != null)
            {
                throw new ArgumentException();
            }

            var templatePositions = template switch
            {
                Template.Cone => throw new NotImplementedException(),
                Template.Wave => throw new NotImplementedException(),
                Template.Hex => TemplateHelper.GetTemplateForHex(size),
                Template.Ring => TemplateHelper.GetTemplateForRing(size),
                Template.Line => direction is null ? throw new ArgumentNullException() : TemplateHelper.GetTemplateForLine(size, direction.Value),
                _ => throw new NotImplementedException()
            };

            return templatePositions.Select(x => x.Add(position)).Where(IsValidPosition).ToList();
        }

        public static bool IsOnSameAxis(IPosition position1, IPosition position2)
        {
            return
                (position1.XPosition == position2.XPosition) ||
                position1.YPosition == position2.YPosition ||
                Math.Abs(position1.XPosition - position2.XPosition) == Math.Abs(position1.YPosition - position2.YPosition) && (position1.XPosition - position2.XPosition) * (position1.YPosition - position2.YPosition) < 0;
        }

        public static bool IsValidPath(List<IPosition> positions)
        {
            if (positions.Count < 2)
            {
                return false;
            }

            return IsValidPosition(positions.First()) && positions
                .Skip(1)
                .Select((position, index) => new { index, position })
                .All(item => IsValidPosition(item.position) && IsAdjacent(item.position, positions[item.index]));
        }

        public static bool IsAdjacent(IPosition position1, IPosition position2)
        {
            return
                (position1.XPosition == position2.XPosition && Math.Abs(position1.YPosition - position2.YPosition) == 1) ||
                (position1.YPosition == position2.YPosition && Math.Abs(position1.XPosition - position2.XPosition) == 1) ||
                (position1.XPosition - position2.XPosition) * (position1.YPosition - position2.YPosition) == -1;
        }

        public static bool IsValidPosition(IPosition position)
        {
            return (Math.Abs(position.XPosition) % 2 == 0) ? Math.Abs(GetNorthValue(position)) <= 8 : Math.Abs(GetNorthValue(position)) <= 9;
        }

        public static int GetNorthValue(IPosition p) => p.XPosition + 2 * p.YPosition;

        // Overaching game rule preferences north and west to break ties, hence this direction is hardcoded 
        public static Comparer<IPosition> NorthWestiestComparer => Comparer<IPosition>.Create((a, b) =>
        {
            var northValueDifference = GetNorthValue(a) - GetNorthValue(b);
            return northValueDifference == 0 ? a.XPosition - b.XPosition : northValueDifference;
        });

        public static IPosition? GetNearest(IPosition position, IEnumerable<IPosition> targets, int startRange = 1, int endRange = MAX_DISTANCE)
        {
            foreach (var ringSize in Enumerable.Range(startRange, endRange))
            {
                var positions = GetTemplate(position, Template.Ring, ringSize);
                var foundTargets = targets.Where(t => positions.Any(p => p.EqualTo(t)));
                if (foundTargets.Any())
                {
                    return foundTargets.Min(NorthWestiestComparer);
                }
            }
            return null;
        }

        public static Direction GetRelativeDirection(IPosition position1, IPosition position2)
        {
            var relativePosition = position2.Subtract(position1);
            if (GetNorthValue(position1) >= GetNorthValue(position2)) //North
            {
                if (relativePosition.XPosition < relativePosition.YPosition)
                {
                    return Direction.NorthWest;
                }
                else if (Math.Abs(relativePosition.XPosition) > Math.Abs(relativePosition.YPosition) / 2)
                {
                    return Direction.NorthEast;
                }
                return Direction.North;
            }
            else //South
            {
                if (relativePosition.XPosition > relativePosition.YPosition)
                {
                    return Direction.SouthEast;
                }
                else if (Math.Abs(relativePosition.XPosition) > Math.Abs(relativePosition.YPosition) / 2)
                {
                    return Direction.SouthWest;
                }
                return Direction.South;
            }
        }

        public static Direction TryAllDirections(Func<Direction, bool> predicate, Direction firstDirection)
        {
            foreach (var direction in new List<Direction>() { firstDirection }.Concat(ORDERED_DIRECTIONS.Where(x => x != firstDirection)))
            {
                if (predicate(direction))
                {
                    return direction;
                }
            }
            throw new Exception("No direction worked");
        }

        public static (Direction, int) TryAllDirectionsAndDistances(Func<Direction, int, bool> predicate, Direction firstDirection)
        {
            foreach (var i in Enumerable.Range(1, MAX_DISTANCE))
            {
                foreach (var direction in new List<Direction>() { firstDirection }.Concat(ORDERED_DIRECTIONS.Where(x => x != firstDirection)))
                {
                    if (predicate(direction, i))
                    {
                        return (direction, i);
                    }
                }

            }
            throw new Exception("No combination of direction and distance worked");
        }

        public static Direction Bounce(Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.South,
                Direction.NorthEast => Direction.NorthWest,
                Direction.NorthWest => Direction.NorthEast,
                Direction.South => Direction.North,
                Direction.SouthEast => Direction.SouthWest,
                Direction.SouthWest => Direction.SouthEast,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static bool IsOverlapping(IPosition target, params IEnumerable<IPosition>[] positions)
        {
            return positions.SelectMany(x => x).Any(p => p.EqualTo(target));
        }
    }
}