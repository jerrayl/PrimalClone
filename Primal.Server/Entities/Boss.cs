using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Primal.Entities
{
    public class Boss : BaseEntity, IPosition
    {
        public int EncounterId { get; set; }
        public int Number { get; set; }
        public Dictionary<string, int> Health { get; set; }
        public int Defence { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public Dictionary<Might, int> Might { get; set; }
        public Direction Direction { get; set; }
        public int? TargetId { get; set; }
        public int? ActionComponentIndex { get; set; }
        public string CustomData { get; set; } = String.Empty;

        public virtual EncounterPlayer Target { get; set; }
        public virtual Encounter Encounter { get; set; }
        public virtual List<BossAction> BossActions { get; set; }
    }
}
