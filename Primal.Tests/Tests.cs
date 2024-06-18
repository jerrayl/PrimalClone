using Primal.Business;
using Primal.Common;
using Primal.Extensions;
using System.Linq;
using Xunit;

namespace PrimalTests
{
    public class RoundOrderTests
    {
        const int FIRST_PLAYER_INDEX = 0;
        const int SECOND_PLAYER_INDEX = 1;
        private GameState startingGameState = new StartingGameStates().ROUND_START_GAME_STATE;

        [Fact]
        public void RoundStarts_InConsumePhase()
        {
            Assert.Equal(RoundPhase.Consume, startingGameState.RoundPhase);
        }

        [Fact]
        public void ConsumingAlemore_HealsPlayer_RemovesPotion()
        {
            var gameState = startingGameState.Copy();

            // Setup 
            gameState.Players[FIRST_PLAYER_INDEX].Damage = 6;
            gameState.Monster.AttritionDiscardPile.Add(new() { Value = 1 });

            // Perform Action
            var newGameState = PlayerActions.UsePotion(gameState, FIRST_PLAYER_INDEX, 0);

            // Update Setup to Expected
            gameState.Players[FIRST_PLAYER_INDEX].Damage = 2;
            gameState.Players[FIRST_PLAYER_INDEX].Potions[0] = null;
            gameState.Players[FIRST_PLAYER_INDEX].HasConsumed = true;

            Assert.Equal(gameState.Serialize(), newGameState.Serialize());
        }

        [Fact]
        public void ConsumingTwice_HasNoEffect()
        {
            var gameState = startingGameState.Copy();

            // Setup 
            gameState.Players[FIRST_PLAYER_INDEX].Damage = 6;
            gameState.Monster.AttritionDiscardPile.Add(new() { Value = 1 });
            gameState.Players[FIRST_PLAYER_INDEX].Potions[1] = 1;

            // Perform Action
            var newGameState = PlayerActions.UsePotion(gameState, FIRST_PLAYER_INDEX, 0);
            newGameState = PlayerActions.UsePotion(newGameState, FIRST_PLAYER_INDEX, 1);

            // Update Setup to Expected
            gameState.Players[FIRST_PLAYER_INDEX].Damage = 2;
            gameState.Players[FIRST_PLAYER_INDEX].Potions[0] = null;
            gameState.Players[FIRST_PLAYER_INDEX].HasConsumed = true;

            Assert.Equal(gameState.Serialize(), newGameState.Serialize());
        }


        [Fact]
        public void MonsterUpkeep_BehaviorRefreshSkippedInFirstRound_OneStruggleGainedPerPlayer()
        {
            var gameState = startingGameState.Copy();
            
            // Setup 
            gameState.RoundPhase = RoundPhase.MonsterUpkeep;
            var clonedGameState = gameState.Copy();

            // Perform Action
            foreach (var playerIndex in Enumerable.Range(0, gameState.Players.Count))
            {
                gameState = PlayerActions.EndRoundPhase(gameState, playerIndex);
            }

            // Update Setup to Expected
            clonedGameState.RoundPhase = RoundPhase.PlayerTurn;
            clonedGameState.Monster.Struggle += clonedGameState.Players.Count;
            clonedGameState.ActivePlayer = FIRST_PLAYER_INDEX;
            clonedGameState.Players[FIRST_PLAYER_INDEX].Tokens.Add(PlayerTokens.FirstPlayer);
            clonedGameState.Players[FIRST_PLAYER_INDEX].TurnPhase = TurnPhase.Movement;

            Assert.Equal(gameState.Serialize(), clonedGameState.Serialize());
        }

        [Fact]
        public void LowestRefreshValueBehavior_IsDiscarded()
        {
            var gameState = startingGameState.Copy();
            
            // Setup 
            gameState.RoundPhase = RoundPhase.MonsterUpkeep;
            gameState.Round = 2;
            gameState.Monster.CurrentBehaviors = [5, 3, 2]; // 2, 4, 6
            gameState.Monster.BehaviorDeck.Insert(0, 1);
            var clonedGameState = gameState.Copy();

            // Perform Action
            foreach (var playerIndex in Enumerable.Range(0, gameState.Players.Count))
            {
                gameState = PlayerActions.EndRoundPhase(gameState, playerIndex);
            }

            // Update Setup to Expected
            clonedGameState.RoundPhase = RoundPhase.PlayerTurn;
            clonedGameState.Monster.CurrentBehaviors = [1, 3, 2];
            clonedGameState.Monster.BehaviorDeck.RemoveAt(0);
            clonedGameState.Monster.BehaviorDiscardPile.Add(5);
            clonedGameState.Monster.Struggle += gameState.Players.Count;
            clonedGameState.ActivePlayer = FIRST_PLAYER_INDEX;
            clonedGameState.Players[FIRST_PLAYER_INDEX].Tokens.Add(PlayerTokens.FirstPlayer);
            clonedGameState.Players[FIRST_PLAYER_INDEX].TurnPhase = TurnPhase.Movement;

            Assert.Equal(gameState.Serialize(), clonedGameState.Serialize());
        }

        [Fact]
        public void LowestRefreshValueBehaviors_WhenTied_AreDiscarded()
        {
            var gameState = startingGameState.Copy();

            // Setup 
            gameState.RoundPhase = RoundPhase.MonsterUpkeep;
            gameState.Round = 2;
            gameState.Monster.CurrentBehaviors = [4, 3, 7]; // 4, 4, 5
            gameState.Monster.BehaviorDeck.Insert(0, 2);
            gameState.Monster.BehaviorDeck.Insert(0, 1);
            var clonedGameState = gameState.Copy();

            // Perform Action
            foreach (var playerIndex in Enumerable.Range(0, gameState.Players.Count))
            {
                gameState = PlayerActions.EndRoundPhase(gameState, playerIndex);
            }

            // Update Setup to Expected
            clonedGameState.RoundPhase = RoundPhase.PlayerTurn;
            clonedGameState.Monster.CurrentBehaviors = [1, 2, 7];
            clonedGameState.Monster.BehaviorDeck.RemoveAt(0);
            clonedGameState.Monster.BehaviorDeck.RemoveAt(0);
            clonedGameState.Monster.BehaviorDiscardPile.Add(4);
            clonedGameState.Monster.BehaviorDiscardPile.Add(3);
            clonedGameState.Monster.Struggle += gameState.Players.Count;
            clonedGameState.ActivePlayer = FIRST_PLAYER_INDEX;
            clonedGameState.Players[FIRST_PLAYER_INDEX].Tokens.Add(PlayerTokens.FirstPlayer);
            clonedGameState.Players[FIRST_PLAYER_INDEX].TurnPhase = TurnPhase.Movement;

            Assert.Equal(gameState.Serialize(), clonedGameState.Serialize());
        }


        [Fact]
        public void WhenStruggleThriceOrMorePlayerCount_UnleashTriggers()
        {
            var gameState = startingGameState.Copy();

            // Setup 
            gameState.RoundPhase = RoundPhase.MonsterUpkeep;
            gameState.Monster.Struggle = 4;
            var clonedGameState = gameState.Copy();

            // Perform Action
            foreach (var playerIndex in Enumerable.Range(0, gameState.Players.Count))
            {
                gameState = PlayerActions.EndRoundPhase(gameState, playerIndex);
            }

            // Update Setup to Expected
            clonedGameState.RoundPhase = RoundPhase.PlayerTurn;
            clonedGameState.Monster.Struggle = 2;
            foreach(var player in clonedGameState.Players)
            {
                player.Damage++;
            }
            clonedGameState.ActivePlayer = FIRST_PLAYER_INDEX;
            clonedGameState.Players[FIRST_PLAYER_INDEX].Tokens.Add(PlayerTokens.FirstPlayer);
            clonedGameState.Players[FIRST_PLAYER_INDEX].TurnPhase = TurnPhase.Movement;

            Assert.Equal(gameState.Serialize(), clonedGameState.Serialize());
        }

        [Fact]
        public void PlayerWithAggroToken_BecomesFirstPlayer()
        {
            var gameState = startingGameState.Copy();

            // Setup 
            gameState.RoundPhase = RoundPhase.MonsterUpkeep;
            gameState.Players[FIRST_PLAYER_INDEX].Tokens.Remove(PlayerTokens.Aggro);
            gameState.Players[SECOND_PLAYER_INDEX].Tokens.Add(PlayerTokens.Aggro);
            var clonedGameState = gameState.Copy();

            // Perform Action
            foreach (var playerIndex in Enumerable.Range(0, gameState.Players.Count))
            {
                gameState = PlayerActions.EndRoundPhase(gameState, playerIndex);
            }

            // Update Setup to Expected
            clonedGameState.RoundPhase = RoundPhase.PlayerTurn;
            clonedGameState.Players[SECOND_PLAYER_INDEX].Tokens.Add(PlayerTokens.FirstPlayer);
            clonedGameState.ActivePlayer = SECOND_PLAYER_INDEX;
            clonedGameState.Players[SECOND_PLAYER_INDEX].TurnPhase = TurnPhase.Movement;
            clonedGameState.Monster.Struggle += clonedGameState.Players.Count;

            Assert.Equal(gameState.Serialize(), clonedGameState.Serialize());
        }


        [Fact]
        public void ActivePlayerSwitches_WhenCurrentPlayerFinishesTurn()
        {
            var gameState = startingGameState.Copy();

            // Setup 
            gameState.RoundPhase = RoundPhase.PlayerTurn;
            gameState.ActivePlayer = FIRST_PLAYER_INDEX;
            gameState.Players[FIRST_PLAYER_INDEX].TurnPhase = TurnPhase.EndOfTurn;
            var clonedGameState = gameState.Copy();

            // Perform Action
            gameState = PlayerActions.EndTurnPhase(gameState, FIRST_PLAYER_INDEX);

            // Update Setup to Expected
            clonedGameState.Players[FIRST_PLAYER_INDEX].HasTakenTurn = true;
            clonedGameState.Players[FIRST_PLAYER_INDEX].TurnPhase = null;
            clonedGameState.ActivePlayer = SECOND_PLAYER_INDEX;
            clonedGameState.Players[SECOND_PLAYER_INDEX].TurnPhase = TurnPhase.Movement;

            Assert.Equal(gameState.Serialize(), clonedGameState.Serialize());
        }

        [Fact]
        public void EndOfRoundTriggers_WhenAllPlayersFinishTurn()
        {
            var gameState = startingGameState.Copy();

            // Setup 
            gameState.RoundPhase = RoundPhase.PlayerTurn;
            gameState.Players[FIRST_PLAYER_INDEX].HasTakenTurn = true;
            gameState.ActivePlayer = SECOND_PLAYER_INDEX;
            gameState.Players[SECOND_PLAYER_INDEX].TurnPhase = TurnPhase.EndOfTurn;
            var clonedGameState = gameState.Copy();

            // Perform Action
            gameState = PlayerActions.EndTurnPhase(gameState, SECOND_PLAYER_INDEX);

            // Update Setup to Expected
            clonedGameState.Players[FIRST_PLAYER_INDEX].HasTakenTurn = false;
            clonedGameState.Players[SECOND_PLAYER_INDEX].TurnPhase = null;
            clonedGameState.ActivePlayer = null;
            clonedGameState.RoundPhase = RoundPhase.EndOfRound;

            Assert.Equal(gameState.Serialize(), clonedGameState.Serialize());
        }

        [Fact]
        public void RoundAdvances_AfterEndOfRoundPhase()
        {
            var gameState = startingGameState.Copy();

            // Setup 
            gameState.RoundPhase = RoundPhase.EndOfRound;
            var clonedGameState = gameState.Copy();

            // Perform Action
            foreach (var playerIndex in Enumerable.Range(0, gameState.Players.Count))
            {
                gameState = PlayerActions.EndRoundPhase(gameState, playerIndex);
            }

            // Update Setup to Expected
            clonedGameState.Round++;
            clonedGameState.RoundPhase = RoundPhase.Consume;

            Assert.Equal(gameState.Serialize(), clonedGameState.Serialize());
        }
    }

    public class Tests
    {
        // ROUND ORDER

        // 1: Consume
        // Player can only consume once per round - DONE
        // Potions are removed when they are consumed - DONE

        // 2: Monster upkeep
        // Behavior refresh is skipped in round 1 - DONE
        // Lowest refresh value behavior of monster is discarded at start of round - DONE
        // All behavior cards with lowest refresh value are discarded when there is a tie - DONE
        // Monster gains 1 struggle per player at start of round - DONE
        // If struggle is equal to higher than 3 * player count, unleash occurs

        // 3: Player turns
        // Player with aggro becomes first player at start of round
        // Next player in turn order has their turn after first player ends their turn

        // 4: End of round
        // Round advances when all players have ended their turn
        // End of round triggers take place at end of round


        // PLAYER TURN

        // 1: Movement
        // Player can only move during their turn during movement phase, or when permitted by monster behavior
        // Player can only move to adjacent space
        // Player must spend one stamina to move
        // Players can only move once during movement phase
        // Player becomes threatened if they do not move
        // Player loses threatened status upon moving

        // 2: Action
        // Player can only use the revive action once per turn
        // Player must pay stamina cost to play card
        // Any surplus stamina generated is lost
        // Damage equal to weapon damage is dealt when playing red card
        // Monster struggle is reduced when playing blue card
        // Played card enters player's sequence
        // Player gains aggro when playing an aggro card
        // Player can only play attacked cards when in an exposed sector
        // Damage dealt is applied to monster
        // Damage above toughness on current stance is inflicted as a wound. Damage equal to toughness is removed
        // Monster advances to next stance when its health reaches the progression value
        // Stance effect triggers when revealed
        // One wound is applied at a time, any future damage is calculated against new toughness value is stance is advanced
        // Player can determine order of triggering abilities from playing a card
        // All game effects must be resolved before another card in the sequence can be played
        // Assisting another player causes them to draw a card
        // Assist can only happen during another player's action phase
        // Each player can only assist once per turn
        // Mastery cards gain counters when triggering condition is met
        // When number of counters is equal to focus value, mastery is focused
        // Reviving another player costs 2 stamina
        // Players gain a stamina token if they end their action phase with two or more cards
        // Players cannot gain more than one stamina unless otherwise specified

        // 3: Attrition
        // Players take attrition damage during attrition phase if they have no defensive cards
        // Damage received is equal to bosses current damage value (with any bonuses)
        // Attrition damage is mitigated by defensive cards in sequence
        // Draw two cards and resolve the higher card if player is threatened
        // Attrition card(s) is discarded after attrition phase
        // Player is knocked out if the damage suffered is more than their health (todo: fill in specifics of KO)
        // Depleted armor does not count towards player's health value
        // Being KO'ed adds wound cards to player's deck

        // 4: End of turn
        // All cards from sequence are discarded
        // Hand is refilled up to hand size limit
        // Player has to discard down to hand size limit
        // Discard pile is reshuffled if deck is emptied
        // Player takes fatigue damage equal to their weapon level if deck is emptied
        // Monster rotates to face aggro player at end of turn


        // BEHAVIOR RESOLUTION
        // When behavior card is triggered, both its base ability and boost (if any) are resolved
        // Struggle is removed from the monster when boost is triggered
        // Boost is not triggered if the monster doesn't have enough struggle
        // Rampage cards are triggered
        // If multiple behavior cards are triggered, the player(s) can choose the resolution order
        // Any event or card ability is that triggers a behavior is fully resolved before the behavior is resolved
        // The behavior card (and rampage card if one is triggered) is discarded and new card(s) are drawn
        // When the behavior deck is empty, the boss gains one struggle and the behavior discard pile is reshuffled, and escalation is triggered
        // If two behavior cards are triggered at the same time, and a rampage card is drawn to replace the first, rampage is triggered
        // If multiple monster cards trigger at the same time, the order of trigger is stance -> peril -> behavior -> other cards, with players deciding the order of simultaneous effects


        // SUCCESS OR FAILURE
        // Players succeed immediately if they reduce the monsters health to zero
        // If all players are KO'ed or the end of round 10 is reached, the players fail the scenario
        // Special failure conditions trigger

    }
}
