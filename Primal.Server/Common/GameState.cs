using System.Collections.Generic;

namespace Primal.Common
{
    public record GameState
    {
        public int Round { get; set; } = 1;
        public RoundPhase RoundPhase { get; set; }
        public int? ActivePlayer { get; set; }
        public Monster Monster { get; set; } = new();
        public List<Player> Players { get; set; } = [];
        public List<int> TriggeredMonsterBehaviors = [];
    }

    public record Monster
    {
        public MonsterType Type { get; set; }
        public int AggressionLevel { get; set; }
        public BoardSector Orientation { get; set; } = BoardSector.South;
        public int Health { get; set; } = GlobalConstants.MONSTER_STARTING_HEALTH;
        public int Damage { get; set; }
        public int Stance { get; set; } = 1;
        public int Struggle { get; set; }
        // TODO: Bonus Damage
        public List<AttritionCard> AttritionDeck { get; set; } = [];
        public List<AttritionCard> AttritionDiscardPile { get; set; } = [];
        public int[] CurrentBehaviors { get; set; } = [];
        public List<int> BehaviorDeck { get; set; } = [];
        public List<int> BehaviorDiscardPile { get; set; } = [];
    }

    public record MonsterDefinition
    {
        public List<StanceCard> StanceCards { get; set; }
        public List<BehaviorCard> BehaviorCards { get; set; }
        public List<PerilObjectiveCard> PerilCards { get; set; }
        public List<PerilObjectiveCard> ObjectiveCards { get; set; }
    }

    public record AttritionCard
    {
        public int Value { get; set; }
    }

    public record BehaviorCard : CardDefinition
    {
        public int AggressionIcon { get; set; }
        public HashSet<BehaviorTrigger> Triggers { get; set; }
        public int RefreshValue { get; set; }
        public MonsterTrait? Trait { get; set; }
        public bool HasBoost { get; set; }
        public int? BoostCost { get; set; }
        public string? BoostAbilityText { get; set; }
    }

    public record StanceCard
    {
        public int AggressionLevel { get; set; }
        public int StanceNumber { get; set; }
        public int Toughness { get; set; }
        public int StanceProgression { get; set; }
        public int Damage { get; set; }
        public HashSet<MonsterSector> Exposure { get; set; }
        public string? TriggerText { get; set; }
    }

    public record PerilObjectiveCard
    {
        public int StanceNumber { get; set; }
        public int AggressionIcon { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AbilityText { get; set; } = string.Empty;
    }

    public record Player
    {
        public ClassType Type { get; set; }
        public BoardSector Location { get; set; } = BoardSector.South;
        public TurnPhase? TurnPhase { get; set; }
        public bool HasTakenTurn { get; set; }
        public bool HasEndedPhase { get; set; }
        public List<int> Sequence { get; set; } = [];
        public List<int> Deck { get; set; } = [];
        public List<int> DiscardPile { get; set; } = [];
        public int Mastery { get; set; }
        public int Damage { get; set; }
        public HashSet<PlayerTokens> Tokens { get; set; } = [];
        public int? Helm { get; set; }
        public int? Armor { get; set; }
        public int? Item { get; set; }
        public int?[] Potions { get; set; } = [null, null, null];
        public bool HasConsumed { get; set; }
        public bool HasUsedRevive { get; set; }
    }

    public record MasteryCardState
    {
        public bool IsFocused { get; set; }
        public int NumCounter { get; set; }
    }

    public record CardDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AbilityText { get; set; } = string.Empty;
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
        public int Level { get; set; }
        public EquipmentType Type { get; set; }
    }

    public record ArmorCard : EquipmentCard
    {
        public int Health { get; set; }
    }
}
