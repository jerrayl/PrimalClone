using System.Collections.Generic;

namespace Primal.Entities
{
    public class EncounterPlayer : BaseEntity, IPosition
    {
        public int EncounterId { get; set; }
        public int PlayerId { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int CurrentHealth { get; set; }
        public int CurrentAnimus { get; set; }
        public Dictionary<Token, int> Tokens { get; set; }

        public virtual Encounter Encounter { get; set; }
        public virtual Player Player { get; set; }
    }
}
