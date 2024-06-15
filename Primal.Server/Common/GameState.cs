using System.Collections.Generic;

namespace Primal.Common
{
    public record GameState
    {
        public int Round { get; set; } = 1;
        public int? ActivePlayer { get; set; }
        public HashSet<int> PlayerTurnOrder { get; set; } = [];
        public Monster Monster { get; set; } = new();
        public List<Player> Players { get; set; } = [];
        public List<int> TriggeredMonsterBehaviors = [];
    }

    public record Monster
    {
        public MonsterType Type { get; set; }
        public BoardSector Orientation { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Stance { get; set; }
        public int Struggle { get; set; }
        // TODO: Bonus Damage
        public List<AttritionCard> AttritionDeck { get; set; } = [];
        public int?[] CurrentBehaviors { get; set; } = [null, null, null];
        public List<int> BehaviorDeck { get; set; } = [];
        public List<int> BehaviorDiscardPile { get; set; } = [];
    }

    public record MonsterDefinition
    {
        public List<StanceCard> StanceCards { get; set; }
        public List<BehaviorCard> BehaviorCards { get; set; }
        public List<PerilCard> PerilCards { get; set; }
        public List<ObjectiveCard> ObjectiveCards { get; set; }
    }

    public record AttritionCard
    {
        public int Value { get; set; }
    }

    public record BehaviorCard : CardDefinition
    {
        public HashSet<BehaviorTrigger> Triggers { get; set; }
        public int RefreshValue { get; set; }
        public MonsterTrait? Trait { get; set; }
        public bool HasBoost { get; set; }
        public int? BoostCost { get; set; }
        public string? BoostAbilityText { get; set; }
    }

    public record StanceCard
    {
        public int StanceNumber { get; set; }
        public int Toughness { get; set; }
        public int StanceProgression { get; set; }
        public int Damage { get; set; }
        public HashSet<MonsterSector> Exposure { get; set; }
    }

    public record PerilCard : CardDefinition
    {
        public int StanceNumber { get; set; }
    }

    public record ObjectiveCard : CardDefinition
    {
        public int StanceNumber { get; set; }
    }

    public record Player
    {
        public ClassType Type { get; set; }
        public TurnPhase? TurnPhase { get; set; }
        public bool HasTakenTurn { get; set; }
        public List<int> Sequence { get; set; } = [];
        public List<int> Deck { get; set; } = [];
        public List<int> DiscardPile { get; set; } = [];
        public int Damage { get; set; }
        public HashSet<PlayerTokens> Tokens { get; set; } = [];
        public int? Helm { get; set; }
        public int? Armor { get; set; }
        public int? Item { get; set; }
        public int?[] Potions { get; set; } = [null, null, null];
        public bool HasConsumed { get; set; }
    }

    public record MasteryCardState
    {
        public bool IsFocused { get; set; }
        public int NumCounter { get; set; }
    }

    public record CardDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AbilityText { get; set; }
    }

    public record PlayerCardDefinition : CardDefinition
    {
        public CardGroup Group { get; set; }
    }

    public record ActionCard : PlayerCardDefinition
    {
        public CardType Type { get; set; }
        public int StaminaCost { get; set; }
        public int StaminaValue { get; set; }
        public bool Aggro { get; set; }
        public PlayerTrait? Trait { get; set; }
    }

    public record MasteryCard : PlayerCardDefinition
    {
        public int FocusValue { get; set; }
    }

    public record WeaponCard : CardDefinition
    {
        public int Level { get; set; }
        public int Damage { get; set; }
        public ClassType ClassType { get; set; }
        public Dictionary<CardActionType, string> ActionDeckComposition { get; set; }
    }

    public record EquipmentCard : CardDefinition
    {
        public EquipmentType Type { get; set; }
        public int? Health { get; set; }
    }
}
