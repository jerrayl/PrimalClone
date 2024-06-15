using System.Collections.Generic;

namespace Primal.Entities
{
    public class Item : BaseEntity
    {
        public string Type { get; set; }
        public int Defence { get; set; }
        public Dictionary<Might, int> Might { get; set; }
        public int Battleflow { get; set; }
        public string Effects { get; set; }
    }
}
