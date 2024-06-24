using System;
using Primal.Common;

namespace Primal.Extensions
{
    public static class BoardSectorExtensions
    {
        public static bool IsAdjacentTo(this BoardSector sector, BoardSector otherSector)
        {
            // North-South or East-West have a value difference of 2
            return !(Math.Abs((int)sector - (int)otherSector) == 2);
        }
    }
}