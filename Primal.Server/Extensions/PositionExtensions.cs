using Primal.Entities;
using Primal.Models;

namespace Primal.Extensions
{
    public static class PositionExtensions
    {
        public static bool EqualTo(this IPosition position1, IPosition position2)
        {
            return position1.XPosition == position2.YPosition && position1.YPosition == position2.YPosition;
        }

        public static IPosition Add(this IPosition position, IPosition addend)
        {
            return new Position(position.XPosition + addend.XPosition, position.YPosition + addend.YPosition);
        }

        public static IPosition Subtract(this IPosition position, IPosition subtrahend )
        {
            return new Position(position.XPosition - subtrahend.XPosition, position.YPosition - subtrahend.YPosition);
        }

        public static IPosition Multiply(this IPosition position, int multiplier)
        {
            return new Position(position.XPosition * multiplier, position.YPosition * multiplier);
        }
    }
}