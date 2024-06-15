using System.Collections.Generic;

namespace Primal.Models
{
    public class PlayerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Class Class { get; set; }
        public int Defence { get; set; }
        public int MaxAnimus { get; set; }
        public int AnimusRegen { get; set; }
        public Dictionary<Might, int> Might { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int CurrentHealth { get; set; }
        public int CurrentAnimus { get; set; }
        public Dictionary<Token, int> Tokens { get; set; }
    }
}