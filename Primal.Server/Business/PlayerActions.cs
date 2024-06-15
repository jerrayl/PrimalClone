using System;
using Primal.Business.Equipment;
using Primal.Common;

namespace Primal.Business
{
    public static class PlayerActions
    {
        public static GameState Move(int playerIndex, BoardSector sector, int? discardedCardId, bool useStaminaToken)
        {
            throw new NotImplementedException();
        }

        public static GameState PlayCard(int playerIndex, int cardId, int[] discardedCardIds, bool useStaminaToken)
        {
            throw new NotImplementedException();
        }

        public static GameState EndPhase(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public static GameState UseEquipmentAction(int playerIndex, EquipmentType equipmentType)
        {
            throw new NotImplementedException();
        }

        public static GameState UsePotion(GameState gameState, int playerIndex, int potionIndex)
        {
            var oldPlayer = gameState.Players[playerIndex];
            if (oldPlayer.HasConsumed || oldPlayer.Potions[potionIndex] is null){
                return gameState;
            }
            
            var newGameState = Potion.Effects[oldPlayer.Potions[potionIndex]!.Value](playerIndex, gameState);
            var player = newGameState.Players[playerIndex];
            player.Potions[potionIndex] = null;
            player.HasConsumed = true;

            return newGameState;
        }
    }
}
