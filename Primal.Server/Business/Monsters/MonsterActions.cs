using System.Linq;
using Primal.Common;
using Primal.Extensions;

namespace Primal.Business.Monsters
{
    public static class MonsterActions
    {
        public static int GetMonsterDamage(GameState gameState)
        {
            var monsterDefinition = MonsterMappings.MonsterMap(gameState.Monster.Type);
            //TODO: Account for bonus damage
            return monsterDefinition.StanceCards.Where(x => x.StanceNumber == gameState.Monster.Stance).Single().Damage;
        }

        public static GameState CheckAndPerformUnleash(GameState gameState)
        {
            if (gameState.Monster.Struggle < gameState.Players.Count * GlobalConstants.MONSTER_UNLEASH_TRIGGER_MULTIPLIER)
            {
                return gameState;
            }

            var damage = GetMonsterDamage(gameState);
            var newGameState = gameState.Copy();

            foreach (var player in newGameState.Players)
            {
                player.Damage += damage;
                // Check for player KO
            }

            newGameState.Monster.Struggle = gameState.Players.Count;

            return newGameState;
        }
    }
}
