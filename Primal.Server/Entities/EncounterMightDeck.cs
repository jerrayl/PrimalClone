using System.Collections.Generic;

namespace Primal.Entities
{
    public class EncounterMightDeck : BaseEntity
    {
        public int EncounterId { get; set; }
        public bool IsFreeCompanyDeck { get; set; }

        public virtual Encounter Encounter { get; set; }
        public virtual List<MightCard> MightCards { get; set; }
    }
}
