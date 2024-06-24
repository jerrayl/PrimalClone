using System.Collections.Generic;
using System.Linq;
using Primal.Business.Monsters;
using Primal.Business.Players;
using Primal.Common;
using Primal.Extensions;

namespace Primal.Business.Helpers
{
    public static class GameStateInitializer
    {
        public static GameState InitializeGameState(MonsterType monsterType, int aggressionLevel, List<ClassType> playersTypes, bool useDefaultLoadout = false)
        {
            var gameState = new GameState();
            var monster = gameState.Monster;
            var monsterDefinition = MonsterMappings.MonsterMap(monsterType);

            // Initialize monster attrition deck
            monster.AttritionDeck = monsterDefinition.AttritionDeck.Select(x => new AttritionCard { Value = x }).ToList();
            monster.AttritionDeck.Shuffle();

            // Initialize monster behavior deck
            monster.BehaviorDeck = monsterDefinition.BehaviorCards.Values.Where(x => MonsterMappings.AggressionIconMap(aggressionLevel).Contains(x.AggressionIcon)).Select(x => x.Id).ToList();
            monster.BehaviorDeck.Shuffle();
            monster.CurrentBehaviors = monster.BehaviorDeck.Take(GlobalConstants.MONSTER_BEHAVIOR_COUNT).ToArray();
            monster.BehaviorDeck.RemoveRange(0, GlobalConstants.MONSTER_BEHAVIOR_COUNT);

            // Initialize players
            foreach (var playerType in playersTypes)
            {
                var playerDefinition = PlayerMappings.PlayerMap(playerType);
                var deck = useDefaultLoadout ? playerDefinition.ActionCards.Where(x => x.Group == CardGroup.S).Select(x => x.Id).ToList() : [];
                deck.Shuffle();
                var hand = deck.Take(GlobalConstants.DEFAULT_HAND_SIZE_LIMIT).ToList();
                deck.RemoveRange(0, GlobalConstants.DEFAULT_HAND_SIZE_LIMIT);
                var mastery = useDefaultLoadout ? playerDefinition.MasteryCards.Where(x => x.Group == CardGroup.S).Single().Id : 0;
                var potions = new int?[] {1, null, null};



                gameState.Players.Add(new Player
                {
                    Type = playerType,
                    Deck = deck,
                    Mastery = mastery,
                    Potions = potions,
                    Hand = hand
                });
            }

            // Todo: Allow players to choose first player
            gameState.Players.First().Tokens.Add(PlayerTokens.Aggro);

            return gameState;
        }
    }
}