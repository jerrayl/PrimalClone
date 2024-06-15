using System;
using System.Collections.Generic;
using System.Linq;

namespace Primal.Business.Helpers
{
    public static class MightCardsHelper
    {
        private static List<int> GetMightPoints(Dictionary<Might, int> might)
        {
            return Enumerable.Repeat(GlobalConstants.BLACK_MIGHT_VALUE, might[Might.Black])
                .Concat(Enumerable.Repeat(GlobalConstants.RED_MIGHT_VALUE, might[Might.Red]))
                .Concat(Enumerable.Repeat(GlobalConstants.YELLOW_MIGHT_VALUE, might[Might.Yellow]))
                .ToList();
        }

        public static int GetEmpowerTokensNeeded(Dictionary<Might, int> mightCards, Dictionary<Might, int> playerMight)
        {
            var playerMightPoints = GetMightPoints(playerMight);
            var mightPointsRequired = GetMightPoints(mightCards).Select((mightPoints, i) =>
                i < GlobalConstants.MAXIMUM_PLAYER_MIGHT - 1 && playerMightPoints.Count > i && mightPoints >= playerMightPoints[i] ?
                mightPoints - playerMightPoints[i] :
                mightPoints
            ).Sum();


            return (int)Math.Ceiling(decimal.Divide(mightPointsRequired, GlobalConstants.EMPOWER_TOKEN_VALUE));
        }
    }
}