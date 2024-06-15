using System.Text.Json.Serialization;

namespace Primal.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BoardSector
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TerrainTypes
    {
        Baethanis,
        Cyricae,
        Brush,
        Fire,
        Fog,
        Sand,
        Plateau,
        Synaerea,
        Water,
        Rock,
        Wildmaw
    }
}
