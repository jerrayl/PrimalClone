using Primal.Business.Helpers;
using Primal.Common;
using Primal.Extensions;

namespace PrimalTests
{
    public class StartingGameStates
    {
        public static readonly GameState ROUND_START_GAME_STATE = GameStateInitializer.InitializeGameState(MonsterType.Vyraxen, 0, [ClassType.GreatBow, ClassType.GreatBow], true);

        public static GameState PLAYER_TURN_GAME_STATE
        {
            get
            {
                var gameState = ROUND_START_GAME_STATE.Copy();
                gameState.RoundPhase = RoundPhase.PlayerTurn;
                gameState.ActivePlayer = 0;
                gameState.Players[0].TurnPhase = TurnPhase.Movement;
                return gameState;
            }
        }
    }
}
