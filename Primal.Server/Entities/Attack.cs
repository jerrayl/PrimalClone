using System.Collections.Generic;

namespace Primal.Entities
{
    public class Attack : BaseEntity
    {
        public int PlayerId { get; set; }
        public int? BossId { get; set; }
        public string BossPart { get; set; }
        public int BonusDamage { get; set; }
        public int EmpowerTokensUsed { get; set; }
        public int RerollTokensUsed { get; set; }

        public virtual Player Player { get; set; }
        public virtual Boss Boss { get; set; }
        public virtual List<AttackMinion> AttackMinions { get; set; }
        public virtual List<MightCard> MightCards { get; set; }
    }
}
