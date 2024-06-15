using System.Collections.Generic;

namespace Primal.Entities
{
    public class Minion : BaseEntity, IPosition
    {
        public int EncounterId { get; set; }
        public int Health { get; set; }
        public int Defence { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public Dictionary<Might, int> Might { get; set; }

        public virtual Encounter Encounter { get; set; }
    }
}
