using System.Collections.Generic;
using Primal.Common;

namespace Primal.Business.Helpers
{
    public interface IMonster
    {
        int[] AttritionDeck { get; }
        Dictionary<int, BehaviorCard> BehaviorCards { get; }
        StanceCard[] StanceCards { get; }
        PerilObjectiveCard[] PerilCards { get; }
        PerilObjectiveCard[] ObjectiveCards { get; }
    }
}