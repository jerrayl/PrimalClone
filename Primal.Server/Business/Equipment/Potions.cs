using System;
using System.Collections.Generic;
using System.Linq;
using Primal.Common;
using Primal.Extensions;

namespace Primal.Business.Equipment
{
    public static class Potion
    {
        public static EquipmentCard[] Potions { get { return [
            new(){
                Id = 1,
                Type = EquipmentType.Potion,
                Level = 1,
                Name = "Alemore",
                AbilityText = "Consume: Choose a player in your sector to heal 2. If the current attrition level is 1 or lower, that player heals 4 instead."
            }
        ];}}

        public static Dictionary<int, Func<int, GameState, GameState>> Effects { get { return new(){
            {1, (playerIndex, gameState) => { 
                var newGameState = gameState.Copy();
                var player = newGameState.Players[playerIndex];
                var lastAttritionCardValue = newGameState.Monster.AttritionDiscardPile.LastOrDefault()?.Value ?? 3;
                player.Damage -= lastAttritionCardValue <= 1 ? 4 : 2;
                if (player.Damage < 0) {
                    player.Damage = 0;
                }
                return newGameState;
            }}
        };}}

    }
}