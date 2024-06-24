using System;
using System.Linq;
using Primal.Business.Equipment;
using Primal.Business.Monsters;
using Primal.Common;
using Primal.Extensions;

namespace Primal.Business.Players
{
    public static class PlayerActions
    {
        public static GameState Move(GameState gameState, BoardSector sector, int? discardedCardIndex, bool useStaminaToken = false)
        {
            if (!gameState.ActivePlayer.HasValue)
            {
                return gameState;
            }

            var player = gameState.Players[gameState.ActivePlayer.Value];
            var playerClassDefinition = PlayerMappings.PlayerMap(player.Type);
            var isNotMovementPhase = player.TurnPhase != TurnPhase.Movement;
            var destinationIsNotAdjacentToCurrentSector = !player.Location.IsAdjacentTo(sector);
            var noStaminaProvided = !discardedCardIndex.HasValue && !useStaminaToken;
            var cardToDiscard = useStaminaToken ? null : playerClassDefinition.ActionCards.Where(card => card.Id == player.Hand[discardedCardIndex!.Value]).Single();
            var invalidCardProvided = !useStaminaToken && (player.Hand.Count < discardedCardIndex || cardToDiscard!.StaminaValue < 1);
            var noStaminaToken = useStaminaToken && !player.Tokens.Contains(PlayerTokens.Stamina);
            var playerHasMoved = player.HasMoved;

            if (isNotMovementPhase || destinationIsNotAdjacentToCurrentSector || noStaminaProvided || invalidCardProvided || playerHasMoved)
            {
                return gameState;
            }

            var newGameState = gameState.Copy();
            var newPlayer = newGameState.Players[newGameState.ActivePlayer!.Value];
            newPlayer.HasMoved = true;
            newPlayer.Tokens.Remove(PlayerTokens.Threatened);
            newPlayer.Location = sector;
            if (useStaminaToken)
            {
                newPlayer.Tokens.Remove(PlayerTokens.Stamina);
            }
            else
            {
                newPlayer.DiscardPile.Add(newPlayer.Hand[discardedCardIndex!.Value]);
                newPlayer.Hand.RemoveAt(discardedCardIndex!.Value);
            }


            return newGameState;
        }

        public static GameState PlayCard(int playerIndex, int cardId, int[] discardedCardIds, bool useStaminaToken)
        {
            throw new NotImplementedException();
        }

        public static GameState EndTurnPhase(GameState gameState)
        {
            if (!gameState.ActivePlayer.HasValue)
            {
                return gameState;
            }

            var newGameState = gameState.Copy();
            var player = newGameState.Players[newGameState.ActivePlayer!.Value];

            switch (player.TurnPhase)
            {
                case TurnPhase.Movement:
                    if (!player.HasMoved)
                    {
                        player.Tokens.Add(PlayerTokens.Threatened);
                    }
                    player.HasMoved = false;
                    player.TurnPhase = TurnPhase.Action;
                    break;
                case TurnPhase.Action:
                    break;
                case TurnPhase.Attrition:
                    break;
                case TurnPhase.EndOfTurn:
                    player.TurnPhase = null;
                    player.HasTakenTurn = true;
                    newGameState.ActivePlayer = null;

                    if (newGameState.Players.All(x => x.HasTakenTurn))
                    {
                        foreach (var otherPlayer in newGameState.Players)
                        {
                            otherPlayer.HasTakenTurn = false;
                        }
                        newGameState.RoundPhase = RoundPhase.EndOfRound;
                        return newGameState;
                    }

                    var nextPlayer = newGameState.Players.Select((player, index) => (player, index)).Where(x => !x.player.HasTakenTurn).First();
                    newGameState.ActivePlayer = nextPlayer.index;
                    nextPlayer.player.TurnPhase = TurnPhase.Movement;
                    break;
            }

            return newGameState;
        }

        public static GameState EndRoundPhase(GameState gameState, int playerIndex)
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
                    var monsterDefinition = MonsterMappings.MonsterMap(newGameState.Monster.Type);
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

                                newGameState = MonsterActions.CheckAndPerformUnleash(newGameState);
                            }
                        }
                    }
                    monster.Struggle += newGameState.Players.Count; //TODO: Account for struggle gain modifier
                    newGameState = MonsterActions.CheckAndPerformUnleash(newGameState);

                    newGameState.RoundPhase = RoundPhase.PlayerTurn;
                    var firstPlayer = newGameState.Players.Select((player, index) => (player, index)).Where(x => x.player.Tokens.Contains(PlayerTokens.Aggro)).Single();
                    newGameState.ActivePlayer = firstPlayer.index;
                    firstPlayer.player.Tokens.Add(PlayerTokens.FirstPlayer);
                    firstPlayer.player.TurnPhase = TurnPhase.Movement;

                    break;
                case RoundPhase.PlayerTurn:
                    //All players ending their turn currently switches to the next phase, so no need to end phase.
                    break;
                case RoundPhase.EndOfRound:
                    newGameState.RoundPhase = RoundPhase.Consume;
                    newGameState.Round++;
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
            if (oldPlayer.HasConsumed || oldPlayer.Potions[potionIndex] is null)
            {
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
