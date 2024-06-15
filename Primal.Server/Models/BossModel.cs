using System.Collections.Generic;

namespace Primal.Models
{
    public class BossModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public Dictionary<string, int> Health { get; set; }
        public int Defence { get; set; }
        public List<BossPartPosition> Positions { get; set; }
        public Dictionary<Might, int> Might { get; set; }
        public List<string> NextAction { get; set; }
        public int? ActionComponentIndex { get; set; }
    }
}