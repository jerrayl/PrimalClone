using System;
using System.Linq;
using Primal.Business.Equipment;
using Primal.Business.Helpers;
using Primal.Common;
using Primal.Extensions;

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

        public static GameState EndPhase(GameState gameState, int playerIndex)
        {
            var newGameState = gameState.Copy();

            newGameState.Players[playerIndex].HasEndedPhase = true;

            if (newGameState.Players.Any(x => !x.HasEndedPhase))
            {
                return newGameState;
            }

            switch (newGameState.RoundPhase)
            {
                case RoundPhase.Consume:
                    newGameState.RoundPhase = RoundPhase.MonsterUpkeep;
                    break;
                case RoundPhase.MonsterUpkeep:
                    var monster = newGameState.Monster;
                    var monsterDefinition = Mappings.MonsterMap(newGameState.Monster.Type);
                    if (newGameState.Round != 1)
                    {
                        var lowestRefreshValue = monster.CurrentBehaviors.Select(cardId => monsterDefinition.BehaviorCards[cardId].RefreshValue).Min();
                        var indexesToDiscard = monster.CurrentBehaviors.Select((cardId, i) => monsterDefinition.BehaviorCards[cardId].RefreshValue == lowestRefreshValue ? i : -1).Where(x => x != -1);
                        monster.BehaviorDiscardPile.AddRange(indexesToDiscard.Select(index => monster.CurrentBehaviors[index]));

                        foreach (var i in indexesToDiscard)
                        {
                            monster.CurrentBehaviors[i] = monster.BehaviorDeck.First();
                            monster.BehaviorDeck.RemoveAt(0);

                            if (monster.BehaviorDeck.Count == 0)
                            {
                                monster.BehaviorDeck = monster.BehaviorDiscardPile.ToList();
                                monster.BehaviorDeck.Shuffle();
                                monster.BehaviorDiscardPile = [];

                                // Perform escalation trigger
                                monster.Struggle += 1;
                                // Check for unleash
                            }
                        }
                    }
                    monster.Struggle += newGameState.Players.Count; //TODO: Account for struggle gain modifier

                    newGameState.RoundPhase = RoundPhase.PlayerTurn;

                    break;
                case RoundPhase.PlayerTurn:
                    // perform end turn
                    break;
                case RoundPhase.EndOfRound:
                    //perform end of round
                    break;
            }

            foreach (var player in newGameState.Players)
            {
                player.HasEndedPhase = false;
            }

            return newGameState;
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
