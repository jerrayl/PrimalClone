using Primal.Business.Helpers;
using Primal.Common;

namespace PrimalTests
{
    public class StartingGameStates
    {
        public readonly GameState ROUND_START_GAME_STATE = GameStateInitializer.InitializeGameState(MonsterType.Vyraxen,  0, [ClassType.GreatBow, ClassType.GreatBow], true);
    }
}
