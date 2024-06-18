using System.Text.Json.Serialization;

namespace Primal
{
    public class GlobalConstants
    {
        // Game constants
        public const int DEFAULT_HAND_SIZE_LIMIT = 5;
        public const int ACTION_CARD_SEQUENCE_LIMIT = 5;
        public const int MONSTER_STARTING_HEALTH = 10;
        public const int GAME_MAX_ROUNDS = 10;
        public const int MONSTER_BEHAVIOR_COUNT = 3;
        public static readonly int[] BASE_ATTRITION_CARDS = [0, 1, 1, 1, 1, 1, 2, 2, 2, 3];
        public const int MONSTER_UNLEASH_TRIGGER_MULTIPLIER = 3;

        // SignalR Responses
        public const string SUCCESS = "SUCCESS";

        // Cookie values
        public const string SCENARIO_ID = "SCENARIOID";
    }
}
