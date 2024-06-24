using System.Text.Json.Serialization;

namespace Primal.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TriggerType
    {
        Revealed,
        EndOfRound,
        BehaviorCardTrait,
        PlayerTurnStart
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TriggerSource
    {
        Mastery,
        Peril
    }
}
