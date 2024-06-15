using System;
using System.Text.Json;
using Primal.Common;

namespace Primal.Extensions
{
    public static class GameStateExtensions
    {
        public static bool Equals(this GameState state, GameState other)
        {
            return JsonSerializer.Serialize(state).Equals(JsonSerializer.Serialize(other));
        }

     
        public static GameState Copy(this GameState state)
        {   
            var serializedState = JsonSerializer.Serialize(state);
            return JsonSerializer.Deserialize<GameState>(serializedState) ?? throw new Exception("Invalid state");
        }
    }
}