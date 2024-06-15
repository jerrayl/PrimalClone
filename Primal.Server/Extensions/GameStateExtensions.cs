using System;
using System.Text.Json;
using Primal.Common;

namespace Primal.Extensions
{
    public static class GameStateExtensions
    {
        public static string Serialize(this GameState state)
        {
            return JsonSerializer.Serialize(state);
        }

     
        public static GameState Copy(this GameState state)
        {   
            var serializedState = JsonSerializer.Serialize(state);
            return JsonSerializer.Deserialize<GameState>(serializedState) ?? throw new Exception("Invalid state");
        }
    }
}