using System;
using System.Collections.Generic;
using System.Linq;
using Primal.Entities;
using Primal.Extensions;
using Primal.Models;

namespace Primal.Business.Helpers
{
    public static class TemplateHelper
    {
        public static IEnumerable<IPosition> GetTemplateForRing(int size)
        {
            if (size == 0)
            {
                return new List<IPosition>() {
                    new Position(0,0)
                };
            }

            var positions = new List<IPosition>();

            foreach (var i in Enumerable.Range(0, size + 1))
            {
                // Northeast side
                positions.Add(new Position(i, size * -1));
                // Southwest side
                positions.Add(new Position(i * -1, size));
            }

            if (size > 0)
            {
                foreach (var i in Enumerable.Range(0, size))
                {
                    // East side
                    positions.Add(new Position(size, i * -1));
                    // West side
                    positions.Add(new Position(size * -1, i));
                }
            }

            if (size > 1)
            {
                foreach (var i in Enumerable.Range(1, size))
                {
                    // Southeast side
                    positions.Add(new Position(size - i, i));
                    // Northwest side
                    positions.Add(new Position(i - size, i * -1));
                }
            }

            return positions;
        }

        public static IEnumerable<IPosition> GetTemplateForHex(int size)
        {
            var positions = new List<IPosition>();

            foreach (var i in Enumerable.Range(0, size))
            {
                positions.Concat(GetTemplateForRing(i));
            }

            return positions;
        }

        public static IEnumerable<IPosition> GetTemplateForLine(int size, Direction direction)
        {
            var vector = direction switch
            {
                Direction.North => new Position(0, -1),
                Direction.NorthEast => new Position(1, -1),
                Direction.NorthWest => new Position(-1, 0),
                Direction.South => new Position(0, 1),
                Direction.SouthEast => new Position(1, 0),
                Direction.SouthWest => new Position(-1, 1),
                _ => throw new ArgumentOutOfRangeException()
            };

            return Enumerable.Range(1, size).Select(i => vector.Multiply(i));
        }
    }
}