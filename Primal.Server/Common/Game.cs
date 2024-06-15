using System.Text.Json.Serialization;

namespace Primal.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoundPhase
    {
        Consume,
        MonsterUpkeep,
        PlayerTurn,
        EndOfRound
    }
}
