using Primal.Business;
using Primal.Common;
using Primal.Extensions;
using Xunit;

namespace PrimalTests
{
    public class RoundOrderTests
    {
        const int FIRST_PLAYER_INDEX = 0;

        [Fact]
        public void RoundStarts_InConsumePhase()
        {
            var startingGameState = new StartingGameStates().ROUND_START_GAME_STATE;

            Assert.Equal(RoundPhase.Consume, startingGameState.RoundPhase);
        }

        [Fact]
        public void ConsumingAlemore_HealsPlayer_RemovesPotion()
        {
            var gameState = new StartingGameStates().ROUND_START_GAME_STATE;

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
            var gameState = new StartingGameStates().ROUND_START_GAME_STATE;

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
    }

    public class Tests
    {
        // ROUND ORDER

        // 1: Consume
        // Player can only consume once per round
        // Potions are removed when they are consumed

        // 2: Monster upkeep
        // Lowest refresh value behavior of monster is discarded at start of round
        // All behavior cards with lowest refresh value are discarded when there is a tie
        // Behavior refresh is skipped in round 1
        // Monster gains 1 struggle per player at start of round
        // If struggle is higher than 3 * player count, unleash occurs

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
        // When the behavior deck is empty, the boss gains one struggle and the behavior discard pile is reshuffled
        // If two behavior cards are triggered at the same time, and a rampage card is drawn to replace the first, rampage is triggered
        // If multiple monster cards trigger at the same time, the order of trigger is stance -> peril -> behavior -> other cards, with players deciding the order of simultaneous effects


        // SUCCESS OR FAILURE
        // Players succeed immediately if they reduce the monsters health to zero
        // If all players are KO'ed or the end of round 10 is reached, the players fail the scenario
        // Special failure conditions trigger

    }
}
