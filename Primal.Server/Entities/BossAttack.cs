using System.Collections.Generic;

namespace Primal.Entities
{
    public class BossAttack : BaseEntity
    {
        public int BossId { get; set; }

        public virtual List<BossAttackPlayer> BossAttackPlayers { get; set; }
        public virtual Boss Boss { get; set; }
        public virtual List<MightCard> MightCards { get; set; }
    }
}
