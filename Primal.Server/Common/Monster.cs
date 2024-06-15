using System.Text.Json.Serialization;

namespace Primal.Common
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MonsterType
    {
        Vyraxen,
        Kharja,
        Toramat,
        Dygorax,
        Korowon,
        Orouxen,
        Felaxir,
        Morkraas,
        Ozew,
        Jekoros,
        Hurom,
        Tarragua,
        Hydar,
        Reikal,
        Pazis,
        Nagarjas,
        Sirkaaj,
        Mamuraak,
        Taraska,
        Xitheros,
        Zekath,
        Zekalith,
        Awakened
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MonsterSector
    {
        Front,
        Rear,
        Flank
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MonsterStatus
    {
        Acceleration,
        Blind,
        Confuse,
        Stun,
        Vulnerable
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BehaviorTrigger
    {
        AttackCard,
        ManeuverCard,
        ParryCard,
        DodgeCard,
        OffensiveCard,
        DefensiveCard,
        ConsecutiveOffensiveCards,
        ConsecutiveDefensiveCards,
        AggroCard,
        EndOfRound,
        Rampage,
        PlayerThreatened,
        FrontSectorPlayerStart,
        FlankSectorPlayerStart,
        RearSectorPlayerStart,
        WaterTerrainPlayerStart,
        MonsterOrientationChange,
        PlayerMovement,
        WaterTerrainPlayerMovement,
        EndOfAttritionPhase,
        PlayerEmptiesHand,
        BehaviorCard,
        FrontSectorAnyPlayer,
        RearSectorAnyPlayer
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MonsterTrait {
        Firestorm
    }
}
