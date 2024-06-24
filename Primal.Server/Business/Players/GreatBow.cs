using Primal.Common;

namespace Primal.Business.Players
{
    public class GreatBow : IPlayerDefinition
    {
        public const string Name = "Mirah"; 
        public ActionCard[] ActionCards { get { return [
            new(){
                Id = 1,
                Name = "Silent Arrow",
                AbilityText = "Stealth. While you are the aggro player, you cannot play this card in your sequence.",
                Group = CardGroup.S,
                Type = CardType.Attack,
                StaminaCost = 0,
                StaminaValue = 1,
                Aggro = false,
                Trait = PlayerTrait.Aim
           },
           new(){
                Id = 2,
                Name = "Precision Draw",
                AbilityText = "Assist. At the start of your turn, if this card is the top card of your discard pile, draw 2.",
                Group = CardGroup.S,
                Type = CardType.Attack,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = true
           },
           new(){
                Id = 3,
                Name = "Deadly Shot",
                AbilityText = "Increase the damage this attack deals by 5[WeaponLevel]. [Mastery]: If this attack deals damage, inflict blind.",
                Group = CardGroup.S,
                Type = CardType.Attack,
                StaminaCost = 3,
                StaminaValue = 2,
                Aggro = true
           },
           new(){
                Id = 4,
                Name = "Heavy Arrow",
                AbilityText = "You may pay 2[Stamina] to increase the damage this attack deals by 6[WeaponLevel]",
                Group = CardGroup.S,
                Type = CardType.Attack,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = true
           },
           new(){
                Id = 5,
                Name = "Blinding Shot",
                AbilityText = "Stealth. If the previous card in your sequence has aim, inflict blind.",
                Group = CardGroup.S,
                Type = CardType.Attack,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 6,
                Name = "Cover Fire",
                AbilityText = "Assist. When another player would suffer attrition damage, you may discard this card from your hand to prevent that damage.",
                Group = CardGroup.S,
                Type = CardType.Attack,
                StaminaCost = 0,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 7,
                Name = "Accurate Shot",
                AbilityText = "Assist.",
                Group = CardGroup.S,
                Type = CardType.Maneuver,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = false,
                Trait = PlayerTrait.Aim
           },
           new(){
                Id = 8,
                Name = "Targeted Shot",
                AbilityText = "[Mastery]: The next time you play a [ManeuverCard] this turn, draw 1.",
                Group = CardGroup.S,
                Type = CardType.Maneuver,
                StaminaCost = 0,
                StaminaValue = 1,
                Aggro = false,
                Trait = PlayerTrait.Aim
           },
           new(){
                Id = 9,
                Name = "Adrenaline Shot",
                AbilityText = "Recycle 1. [Mastery]: Volley 2.",
                Group = CardGroup.S,
                Type = CardType.Maneuver,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 10,
                Name = "Rapid Burst",
                AbilityText = "Assist. Volley 2",
                Group = CardGroup.S,
                Type = CardType.Maneuver,
                StaminaCost = 1,
                StaminaValue = 2,
                Aggro = true
           },
           new(){
                Id = 11,
                Name = "Sharp Shot",
                AbilityText = "[Mastery]: The next time you play a [AttackCard] this turn, deal 2[WeaponLevel] damage.",
                Group = CardGroup.S,
                Type = CardType.Maneuver,
                StaminaCost = 0,
                StaminaValue = 1,
                Aggro = false,
                Trait = PlayerTrait.Aim
           },
           new(){
                Id = 12,
                Name = "Bouncing Shot",
                AbilityText = "When you discard this card from your hand, perform volley 2.",
                Group = CardGroup.S,
                Type = CardType.Maneuver,
                StaminaCost = 0,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 13,
                Name = "Ranged Position",
                AbilityText = "Assist. While this card is the top card of your discard pile, it counts as a [DodgeCard] in your sequence for the attrition check.",
                Group = CardGroup.S,
                Type = CardType.Dodge,
                StaminaCost = 0,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 14,
                Name = "Sneaky Glide",
                AbilityText = "Recycle 1. The next card you play in your sequence gains stealth.",
                Group = CardGroup.S,
                Type = CardType.Dodge,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 15,
                Name = "Acrobatics",
                AbilityText = "[FollowedBy][ManeuverCard]: Draw 3 and then you may immediately move without paying any stamina cost.",
                Group = CardGroup.S,
                Type = CardType.Dodge,
                StaminaCost = 2,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 16,
                Name = "Evasion",
                AbilityText = "Draw 2",
                Group = CardGroup.S,
                Type = CardType.Dodge,
                StaminaCost = 2,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 17,
                Name = "Long Range",
                AbilityText = "The next [AttackCard] you play in your sequence gains stealth. While this card is the top card of your discard pile, prevent any attrition damage you would suffer.",
                Group = CardGroup.S,
                Type = CardType.Dodge,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 18,
                Name = "Backflip",
                AbilityText = "You can only play this card after a [AttackCard]. The next time you refill your hand, draw 1.",
                Group = CardGroup.S,
                Type = CardType.Dodge,
                StaminaCost = 0,
                StaminaValue = 2,
                Aggro = false
           },
           new(){
                Id = 19,
                Name = "Lightning Escape",
                AbilityText = "Search your discard pile for a [DodgeCard] and add it to your hand.",
                Group = CardGroup.S,
                Type = CardType.Parry,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 20,
                Name = "Graceful Defense",
                AbilityText = "Reduce the stamina cost of each [DefensiveCard] you play this turn by 1.",
                Group = CardGroup.S,
                Type = CardType.Parry,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = true
           },
           new(){
                Id = 21,
                Name = "Intuition",
                AbilityText = "When you play a [DefensiveCard], you may recycle this card from your hand.",
                Group = CardGroup.S,
                Type = CardType.Parry,
                StaminaCost = 1,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 22,
                Name = "Swift Deflection",
                AbilityText = "When you discard this card from your hand, draw 1.",
                Group = CardGroup.S,
                Type = CardType.Parry,
                StaminaCost = 0,
                StaminaValue = 1,
                Aggro = false
           },
           new(){
                Id = 23,
                Name = "Prompt Reflexes",
                AbilityText = "Draw 1.",
                Group = CardGroup.S,
                Type = CardType.Parry,
                StaminaCost = 1,
                StaminaValue = 0,
                Aggro = false
           },
           new(){
                Id = 24,
                Name = "Decoy",
                AbilityText = "Choose a player to draw 1.",
                Group = CardGroup.S,
                Type = CardType.Parry,
                StaminaCost = 1,
                StaminaValue = 2,
                Aggro = true
           }
        ];}}

        public MasteryCard[] MasteryCards { get { return [
        new(){
                Id = 1,
                Name = "Ultimate Shot",
                AbilityText = "Start: Volley 2. When you empty your deck, deal 5[WeaponLevel] damage.",
                Group = CardGroup.S,
                FocusValue = 1
           }
        ];}}
    }
}