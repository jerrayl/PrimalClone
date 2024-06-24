using System.Collections.Generic;
using System.Linq;
using Primal.Common;

namespace Primal.Business.Monsters
{
    public class Vyraxen : IMonsterDefinition
    {
        public int[] AttritionDeck { get { return GlobalConstants.BASE_ATTRITION_CARDS; } }
        public Dictionary<int, BehaviorCard> BehaviorCards { get { return new List<BehaviorCard>(){
            new(){
                Id = 1,
                AggressionIcon = 0,
                Name = "Flaming Gaze",
                AbilityText = "The active player becomes threatened",
                Triggers = [BehaviorTrigger.DodgeCard],
                RefreshValue = 1
            },
            new(){
                Id = 2,
                AggressionIcon = 0,
                Name = "Aggression",
                AbilityText = "Each player in the front sector must either move or suffer [MonsterDamage]. Then Vyraxen turns to the player with fewest [DefensiveCard] in their hand",
                Triggers = [BehaviorTrigger.EndOfRound],
                RefreshValue = 6,
            },
            new(){
                Id = 3,
                AggressionIcon = 0,
                Name = "Wrath of the Dragon",
                AbilityText = "Vyraxen turns to the active player. Then place a fire terrain in the front sector.",
                Triggers = [BehaviorTrigger.ManeuverCard],
                RefreshValue = 4,
                HasBoost = true,
                BoostCost = 1,
                BoostAbilityText = "The active player suffers [MonsterDamage] unless they have a [DodgeCard] in their sequence."
            },
            new(){
                Id = 4,
                AggressionIcon = 1,
                Name = "Blazing Assault",
                AbilityText = "The active player discards the top 3 cards of their deck. That player suffers [MonsterDamage] unless a [DefensiveCard] is discarded this way.",
                Triggers = [BehaviorTrigger.AttackCard],
                RefreshValue = 4,
                HasBoost = true,
                BoostCost = 2,
                BoostAbilityText = "Repeat the base ability effect of this card."
            },
            new(){
                Id = 5,
                AggressionIcon = 1,
                Name = "Smouldering Breath",
                AbilityText = "Vyraxen gains 1[PlayerCount] struggle",
                Triggers = [BehaviorTrigger.PlayerThreatened],
                RefreshValue = 2,
                Trait = MonsterTrait.Firestorm
            },
            new(){
                Id = 6,
                AggressionIcon = 1,
                Name = "Sudden Blaze",
                AbilityText = "The active player discards the top 3 cards of their deck. That player suffers [MonsterDamage] unless a [DefensiveCard] is discarded this way.",
                Triggers = [BehaviorTrigger.AttackCard],
                RefreshValue = 2,
                Trait = MonsterTrait.Firestorm
            },
            new(){
                Id = 7,
                AggressionIcon = 2,
                Name = "Flamethrower",
                AbilityText = "Place a fire terrain in the front sector",
                Triggers = [BehaviorTrigger.Rampage],
                RefreshValue = 5,
                HasBoost = true,
                BoostCost = 1,
                BoostAbilityText = "Each player in the front sector suffers [MonsterDamage] unless they discard a [DodgeCard] from their hand."
            },
            new(){
                Id = 8,
                AggressionIcon = 2,
                Name = "Thunderous Growl",
                AbilityText = "Vyraxen gains [PlayerCount]-1 struggle and gets [BonusDamage]1 until the end of this round.",
                Triggers = [BehaviorTrigger.FrontSectorPlayerStart],
                RefreshValue = 7
            },
            new(){
                Id = 9,
                AggressionIcon = 2,
                Name = "Whipping Tail",
                AbilityText = "The active player suffers [MonsterDamage] unless they discard a [DodgeCard] from their hand. If the hit the tail objective is completed, this card has no effect. If so, remove it from the game.",
                Triggers = [BehaviorTrigger.RearSectorPlayerStart],
                RefreshValue = 3
            }
        }.ToDictionary(x => x.Id, x => x);}}

        public StanceCard[] StanceCards { get { return [
            new(){
                AggressionLevel = 0,
                StanceNumber = 1,
                Toughness = 2,
                StanceProgression = 7,
                Damage = 1,
                Exposure = [MonsterSector.Front, MonsterSector.Rear]
            },
            new(){
                AggressionLevel = 0,
                StanceNumber = 2,
                Toughness = 3,
                StanceProgression = 3,
                Damage = 1,
                Exposure = [MonsterSector.Flank, MonsterSector.Rear],
                TriggerText = "When Revealed: Vyraxen gains 1[PlayerCount] struggle."
            },
            new(){
                AggressionLevel = 0,
                StanceNumber = 3,
                Toughness = 4,
                StanceProgression = 0,
                Damage = 2,
                Exposure = [MonsterSector.Flank, MonsterSector.Front, MonsterSector.Rear],
                TriggerText = "When Revealed: Place a fire terrain in each threatened sector."
            }
        ];}}


        public PerilObjectiveCard[] PerilCards { get { return [
            new(){
                StanceNumber = 1,
                AggressionIcon = 2,
                Name = "Fire Breathing",
                AbilityText = "At the end of the round, if the aggro player is threatened, place a fire terrain in the front sector."
            },
            new(){
                StanceNumber = 2,
                AggressionIcon = 2,
                Name = "Wall of Flames",
                AbilityText = "When a behavior card with firestorm is revealed, immediately place a fire terrain in the front sector."
            },
            new(){
                StanceNumber = 3,
                AggressionIcon = 2,
                Name = "Inferno",
                AbilityText = "When a behavior card with firestorm is revealed, immediately place a fire terrain in the front sector. Players in that sector become threatened."
            }
        ];}}

        public PerilObjectiveCard[] ObjectiveCards { get { return [
            new(){
                StanceNumber = 1,
                AggressionIcon = 2,
                Name = "Hit the Tail",
                AbilityText = "Permanent. [ConsecutiveOffensiveCard]: If you are in the rear sector, place a counter on this card. When there are 2[PlayerCount] counters on this card, Vyraxen suffers 2 wounds. Then remove this card from the game."
            },
            new(){
                StanceNumber = 2,
                AggressionIcon = 2,
                Name = "Hit the Tail",
                AbilityText = "Permanent. [ConsecutiveOffensiveCard]: If you are in the rear sector, place a counter on this card. When there are 2[PlayerCount] counters on this card, Vyraxen suffers 2 wounds. Then remove this card from the game."
            },
            new(){
                StanceNumber = 3,
                AggressionIcon = 2,
                Name = "Hit the Tail",
                AbilityText = "Permanent. [ConsecutiveOffensiveCard]: If you are in the rear sector, place a counter on this card. When there are 2[PlayerCount] counters on this card, Vyraxen suffers 2 wounds. Then remove this card from the game."
            }
        ];}}
    }
}