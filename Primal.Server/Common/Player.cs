using System.Text.Json.Serialization;

namespace Primal.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ClassType
    {
        GreatSword,
        GreatBow,
        Hammer,
        SwordAndShield,
        DualBlade,
        HeavyGun
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CardActionType
    {
        Offensive,
        Defensive,
        Wound
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CardType
    {
        Attack,
        Maneuver,
        Dodge,
        Parry,
        Wound
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlayerTrait
    {
        Slash,
        Aim
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlayerTokens
    {
        Aggro,
        FirstPlayer,
        Threatened,
        Deplete,
        Burning,
        Disrupt,
        Dazed,
        Defense,
        KO,
        Stamina,
        Strain
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TurnPhase
    {
        Movement,
        Action,
        Attrition,
        EndOfTurn
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EquipmentType
    {
        Helm,
        Armor,
        Item,
        Potion
    }

    public enum CardGroup
    {
        S,
        A1,
        A2,
        B1,
        B2,
        C1,
        C2,
        D1,
        D2,
        E1,
        E2
    }

    public class PlayerHelpers
    {
        public static CardActionType GetCardActionTypeFromCardType(CardType cardType)
        {
            if (cardType == CardType.Attack || cardType == CardType.Maneuver)
            {
                return CardActionType.Offensive;
            }
            else if (cardType == CardType.Dodge || cardType == CardType.Parry)
            {
                return CardActionType.Defensive;
            }
            else
            {
                return CardActionType.Wound;
            }
        }
    }
}
