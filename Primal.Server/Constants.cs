using System.Text.Json.Serialization;

namespace Primal
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Might
    {
        White = 1,
        Yellow = 2,
        Red = 3,
        Black = 4
    }

    public enum BossPart
    {
        Front,
        Rear,
        Core,
        Flank,
        LeftFlank,
        RightFlank,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Token
    {
        Battleflow,
        Redraw,
        Defence,
        Empower,
        Animus
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Class
    {
        Warbear,
        Exile,
        Cur,
        Priest,
        Harbinger,
        Blade,
        Witch,
        Ranger,
        Warden,
        Huntress,
        Penitent,
        GroveMaiden
    }

    public enum Template
    {
        NarrowCone,
        Cone,
        Hex,
        Ring,
        Line,
        Wave
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Direction
    {
        NorthWest,
        North,
        NorthEast,
        SouthEast,
        South,
        SouthWest
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Corner
    {
        NorthWest,
        NorthEast,
        East,
        SouthEast,
        SouthWest,
        West
    }

    public enum Size
    {
        Normal,
        Large
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CharacterType
    {
        Player,
        Boss,
        Minion
    }

    public class GlobalConstants
    {
        // Game constants
        public const int MAXIMUM_HEALTH = 6;
        public const int EMPOWER_TOKEN_VALUE = 3;
        public const int ANIMUS_TOKEN_VALUE = 2;
        public const int NUM_ZEROES_TO_MISS = 2;
        public const int MAXIMUM_ATTACK_MIGHT_CARDS = 14;
        public const int MAXIMUM_PLAYER_MIGHT = 4;
        public const int BLACK_MIGHT_VALUE = 3;
        public const int RED_MIGHT_VALUE = 2;
        public const int YELLOW_MIGHT_VALUE = 1;

        // SignalR Responses
        public const string SUCCESS = "SUCCESS";

        // Cookie values
        public const string ENCOUNTER_ID = "ENCOUNTERID";
    }
}
